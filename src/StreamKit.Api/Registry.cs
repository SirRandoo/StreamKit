// MIT License
// 
// Copyright (c) 2022 SirRandoo
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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace StreamKit.Api;

/// <summary>
///     A generic registry for housing a list of objects.
/// </summary>
/// <typeparam name="T">The class type being housed within the registry</typeparam>
public class Registry<T> where T : class, IIdentifiable
{
    private const int LockTimeout = 300;
    private readonly ReaderWriterLockSlim _lock = new();
    private readonly List<T> _registrants = new();
    private readonly Dictionary<string, T> _registrantsKeyed = new();

    /// <summary>
    ///     Returns a list of to the objects within the registry.
    /// </summary>
    public List<T> AllRegistrants
    {
        get
        {
            if (!_lock.TryEnterReadLock(LockTimeout))
            {
                return new List<T>(0);
            }

            var container = new List<T>(_registrants);

            _lock.ExitReadLock();

            return container;
        }
    }

    /// <summary>
    ///     Gets an object registered within the registry by its id.
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns>
    ///     The object if it exists, or <see langword="null"/> if it doesn't.
    /// </returns>
    public T? Get(string id)
    {
        if (!_lock.TryEnterReadLock(LockTimeout))
        {
            return default;
        }

        if (!_registrantsKeyed.TryGetValue(id, out T value))
        {
            _lock.ExitReadLock();

            return default;
        }

        _lock.ExitReadLock();

        return value;
    }

    /// <summary>
    ///     Registers an object to the registry.
    /// </summary>
    /// <param name="obj">The object to register</param>
    public void Register([NotNull] T obj)
    {
        if (!_lock.TryEnterWriteLock(LockTimeout))
        {
            return;
        }

        if (!_registrantsKeyed.TryAdd(obj.Id, obj))
        {
            _lock.ExitWriteLock();

            return;
        }

        _registrants.Add(obj);

        _lock.ExitWriteLock();
    }

    /// <summary>
    ///     Unregisters an object from the registry.
    /// </summary>
    /// <param name="obj">The object to unregister.</param>
    /// <returns>Whether the object was unregistered.</returns>
    public bool Unregister(T obj)
    {
        if (!_lock.TryEnterWriteLock(LockTimeout))
        {
            return false;
        }

        bool removed = _registrants.Remove(obj) && _registrantsKeyed.Remove(obj.Id);

        _lock.ExitWriteLock();

        return removed;
    }

    /// <summary>
    ///     Unregisters an object from the registry.
    /// </summary>
    /// <param name="id">The id of the object to unregister.</param>
    /// <returns>Whether the object was unregistered.</returns>
    public bool Unregister(string id)
    {
        if (!_lock.TryEnterUpgradeableReadLock(LockTimeout))
        {
            return false;
        }

        if (!_registrantsKeyed.TryGetValue(id, out T value))
        {
            _lock.ExitUpgradeableReadLock();

            return false;
        }

        if (_lock.TryEnterWriteLock(LockTimeout))
        {
            _lock.ExitUpgradeableReadLock();

            return false;
        }

        bool removed = _registrants.Remove(value) && _registrantsKeyed.Remove(id);

        _lock.ExitWriteLock();
        _lock.ExitUpgradeableReadLock();

        return removed;
    }
}
