// MIT License
//
// Copyright (c) 2024 SirRandoo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace StreamKit.Mod.Integration.Twitch;

public class ExpirableList<T> : IList<T>
{
    private readonly List<T> _container = [];
    private readonly Dictionary<T, DateTime> _itemExpirations = [];
    private readonly TimeSpan _lifetime;

    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.NoRecursion);
    private readonly Timer _timer;

    public ExpirableList(TimeSpan lifetime)
    {
        _lifetime = lifetime;
        _timer = new Timer(CleanContainers, null, Timeout.InfiniteTimeSpan, TimeSpan.FromMinutes(5));
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        _lock.EnterReadLock();

        List<T>.Enumerator enumerator = _container.GetEnumerator();

        _lock.ExitReadLock();

        return enumerator;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public void Add(T item)
    {
        _lock.EnterWriteLock();

        _container.Add(item);
        _itemExpirations.Add(item, DateTime.UtcNow);

        _lock.ExitWriteLock();
    }

    /// <inheritdoc />
    public void Clear()
    {
        _lock.EnterWriteLock();

        _container.Clear();
        _itemExpirations.Clear();

        _lock.ExitWriteLock();
    }

    /// <inheritdoc />
    public bool Contains(T item)
    {
        _lock.EnterReadLock();

        bool contains = _itemExpirations.ContainsKey(item);

        _lock.ExitReadLock();

        return contains;
    }

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex)
    {
        _lock.EnterWriteLock();

        _container.CopyTo(array, arrayIndex);

        _lock.ExitWriteLock();
    }

    /// <inheritdoc />
    public bool Remove(T item)
    {
        _lock.EnterWriteLock();

        bool removed = _container.Remove(item);

        if (_itemExpirations.ContainsKey(item))
        {
            removed = _itemExpirations.Remove(item) && removed;
        }

        _lock.ExitWriteLock();

        return removed;
    }

    /// <inheritdoc />
    public int Count
    {
        get
        {
            _lock.EnterReadLock();

            int count = Math.Max(_itemExpirations.Count, _container.Count);

            _lock.ExitReadLock();

            return count;
        }
    }

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public int IndexOf(T item)
    {
        _lock.EnterReadLock();

        int index = _container.IndexOf(item);

        _lock.ExitReadLock();

        return index;
    }

    /// <inheritdoc />
    public void Insert(int index, T item)
    {
        _lock.EnterWriteLock();

        _container.Insert(index, item);
        _itemExpirations[item] = DateTime.UtcNow;

        _lock.ExitWriteLock();
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        _lock.EnterWriteLock();

        T item = _container[index];
        _container.RemoveAt(index);
        _itemExpirations.Remove(item);

        _lock.ExitWriteLock();
    }

    /// <inheritdoc />
    public T this[int index]
    {
        get
        {
            _lock.EnterReadLock();

            T item = _container[index];

            _lock.ExitReadLock();

            return item;
        }
        set
        {
            _lock.EnterWriteLock();

            _container[index] = value;

            _lock.ExitWriteLock();
        }
    }

    private void CleanContainers(object state)
    {
        if (!_lock.TryEnterWriteLock(500))
        {
            return;
        }

        for (int i = _container.Count - 1; i >= 0; i--)
        {
            T item = _container[i];

            if (!_itemExpirations.TryGetValue(item, out DateTime added) || (DateTime.UtcNow - added).TotalMilliseconds > _lifetime.TotalMilliseconds)
            {
                continue;
            }

            _container.RemoveAt(i);
            _itemExpirations.Remove(item);
        }
    }
}
