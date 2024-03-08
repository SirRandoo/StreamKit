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

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api;

/// <summary>
///     A registry implementation that uses
///     <see
///         cref="Interlocked.CompareExchange(ref object, object, object)" />
///     to modify the registry's contents.
/// </summary>
/// <typeparam name="T">
///     The type of the class being represented within the registry.
/// </typeparam>
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public class SynchronisedRegistry<T> : IRegistry<T> where T : class, IIdentifiable
{
    private readonly ConcurrentDictionary<string, T> _allRegistrantsKeyed = [];
    private ImmutableList<T> _allRegistrants;

    /// <summary>
    ///     A registry implementation that uses
    ///     <see
    ///         cref="Interlocked.CompareExchange(ref object, object, object)" />
    ///     to modify the registry's contents.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the class being represented within the registry.
    /// </typeparam>
    public SynchronisedRegistry(ImmutableList<T>? allRegistrants = default)
    {
        _allRegistrants = allRegistrants ?? ImmutableList<T>.Empty;

        for (var i = 0; i < _allRegistrants.Count; i++)
        {
            T registrant = _allRegistrants[i];

            _allRegistrantsKeyed.TryAdd(registrant.Id, registrant);
        }
    }

    /// <inheritdoc />
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public IList<T> AllRegistrants => _allRegistrants;

    /// <inheritdoc />
    public bool Register([NotNull] T obj)
    {
        bool added = ImmutableInterlocked.Update(ref _allRegistrants, list => list.Add(obj));

        return _allRegistrantsKeyed.TryAdd(obj.Id, obj) && added;
    }

    /// <inheritdoc />
    public bool Unregister(T obj)
    {
        bool removed = ImmutableInterlocked.Update(ref _allRegistrants, list => list.Remove(obj));

        return _allRegistrantsKeyed.TryRemove(obj.Id, out T? _) && removed;
    }

    /// <inheritdoc />
    public T? Get(string id) => _allRegistrantsKeyed.TryGetValue(id, out T? value) ? value : default;
}
