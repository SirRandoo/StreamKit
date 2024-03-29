// MIT License
//
// Copyright (c) 2023 SirRandoo
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
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api;

public class MutableRegistry<T> : IRegistry<T> where T : class, IIdentifiable
{
    private readonly IList<T> _allRegistrants;
    private readonly Dictionary<string, T> _allRegistrantsKeyed = new();

    public MutableRegistry(IList<T>? allRegistrants = default)
    {
        _allRegistrants = allRegistrants ?? [];

        for (var i = 0; i < _allRegistrants.Count; i++)
        {
            T registrant = _allRegistrants[i];

            _allRegistrantsKeyed[registrant.Id] = registrant;
        }
    }

    /// <inheritdoc />
    public IList<T> AllRegistrants => new ReadOnlyCollection<T>(_allRegistrants);

    /// <inheritdoc />
    public bool Register([NotNull] T obj)
    {
        if (_allRegistrantsKeyed.ContainsKey(obj.Id))
        {
            return false;
        }

        _allRegistrants.Add(obj);
        _allRegistrantsKeyed[obj.Id] = obj;

        return true;
    }

    /// <inheritdoc />
    public bool Unregister(T obj)
    {
        if (!_allRegistrantsKeyed.ContainsKey(obj.Id))
        {
            return false;
        }

        bool listRemoved = _allRegistrants.Remove(obj);
        bool dictRemoved = _allRegistrantsKeyed.Remove(obj.Id);

        return listRemoved && dictRemoved;
    }

    /// <inheritdoc />
    public T? Get(string id) => _allRegistrantsKeyed.TryGetValue(id, out T? value) ? value : default;
}
