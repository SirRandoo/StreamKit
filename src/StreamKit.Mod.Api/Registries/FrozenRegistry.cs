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
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api;

public class FrozenRegistry<T> : IRegistry<T> where T : class, IIdentifiable
{
    private readonly ImmutableList<T> _registrants;
    private readonly ImmutableDictionary<string, T> _registrantsKeyed;

    public FrozenRegistry(IList<T> allRegistrants)
    {
        _registrants = ImmutableList.CreateRange(allRegistrants);

        var dict = new Dictionary<string, T>();

        for (var index = 0; index < allRegistrants.Count; index++)
        {
            T registrant = allRegistrants[index];
            dict[registrant.Id] = registrant;
        }

        _registrantsKeyed = dict.ToImmutableDictionary();
    }

    /// <inheritdoc />
    public ICollection<T> AllRegistrants => _registrants;

    /// <summary>
    ///     <inheritdoc cref="IRegistry{T}.Register" Path="/summary" />
    /// </summary>
    /// <param name="obj">
    ///     <inheritdoc cref="IRegistry{T}.Register"
    ///                 Path="/param[name='obj']" />
    /// </param>
    /// <exception cref="ReadOnlyException">
    ///     Thrown when the registry is no longer accepting modifications.
    /// </exception>
    /// <remarks>
    ///     This method will always throw <see cref="ReadOnlyException" /> as
    ///     the registry is permanently frozen after initialization. If
    ///     modifications are necessary, developers should create a new
    ///     <see cref="FrozenRegistry{T}" />, or use a mutable registry.
    /// </remarks>
    public bool Register([NotNull] T obj) => throw new ReadOnlyException();

    /// <summary>
    ///     <inheritdoc cref="IRegistry{T}.Register" Path="/summary" />
    /// </summary>
    /// <param name="obj">
    ///     <inheritdoc cref="IRegistry{T}.Register"
    ///                 Path="/param[name='obj']" />
    /// </param>
    /// <exception cref="ReadOnlyException">
    ///     Thrown when the registry is no longer accepting modifications.
    /// </exception>
    /// <remarks>
    ///     This method will always throw <see cref="ReadOnlyException" /> as
    ///     the registry is permanently frozen after initialization. If
    ///     modifications are necessary, developers should create a new
    ///     <see cref="FrozenRegistry{T}" />, or use a mutable registry.
    /// </remarks>
    public bool Unregister(T obj) => throw new ReadOnlyException();

    /// <inheritdoc />
    public T? Get(string id) => _registrantsKeyed.TryGetValue(id, out T? value) ? value : default;
}
