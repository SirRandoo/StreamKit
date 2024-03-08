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

using System;
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
public class SynchronisedRegistry<T>(IList<T>? allRegistrants = default) : IRegistry<T> where T : class, IIdentifiable
{
    private IList<T> _allRegistrants = allRegistrants ?? ImmutableList<T>.Empty;

    /// <inheritdoc />
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public IList<T> AllRegistrants => _allRegistrants;

    /// <inheritdoc />
    public bool Register([NotNull] T obj)
    {
        ImmutableList<T> modified, original;

        do
        {
            original = (ImmutableList<T>)AllRegistrants;
            modified = original.Add(obj);
        } while (Interlocked.CompareExchange(ref _allRegistrants, modified, original) != original);

        return true;
    }

    /// <inheritdoc />
    public bool Unregister(T obj)
    {
        ImmutableList<T> modified, original;

        do
        {
            original = (ImmutableList<T>)AllRegistrants;
            modified = original.Remove(obj);
        } while (Interlocked.CompareExchange(ref _allRegistrants, modified, original) != original);

        return true;
    }

    /// <inheritdoc />
    public T? Get(string id)
    {
        return ((ImmutableList<T>)AllRegistrants).Find(r => string.Equals(id, r.Id, StringComparison.Ordinal));
    }
}
