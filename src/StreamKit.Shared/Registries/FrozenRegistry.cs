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

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Shared.Registries;

/// <summary>
///     Provides an immutable registry for identifiable objects of type <typeparamref name="T" />.
///     This registry ensures that once initialized, no further modifications can be made.
/// </summary>
/// <typeparam name="T">
///     The type of objects stored in the registry. Must implement
///     <see cref="IIdentifiable" />.
/// </typeparam>
/// <remarks>
///     The <see cref="FrozenRegistry{T}" /> is ideal for scenarios where read-only access to a
///     predefined collection of objects is required, such as configuration data or static game assets.
///     It ensures thread-safety by making the underlying collections immutable.
/// </remarks>
/// <example>
///     Example usage:
///     <code>
/// var items = new List&lt;SomeIdentifiableType&gt;
/// {
///     new SomeIdentifiableType("ID1"),
///     new SomeIdentifiableType("ID2")
/// };
/// var registry = FrozenRegistry&gt;SomeIdentifiableType&gt;.CreateInstance(items);
/// var item = registry.Get("ID1");
/// </code>
/// </example>
[PublicAPI]
public sealed class FrozenRegistry<T> : IReadOnlyRegistry<T> where T : class, IIdentifiable
{
    private ImmutableList<T> _registrants = null!;
    private ImmutableDictionary<string, T> _registrantsKeyed = null!;

    private FrozenRegistry()
    {
    }

    /// <inheritdoc />
    public IReadOnlyList<T> AllRegistrants => _registrants;

    /// <inheritdoc />
    public T? Get(string id) => _registrantsKeyed.TryGetValue(id, out T? value) ? value : default;

    /// <summary>
    ///     Creates a new instance of a <see cref="FrozenRegistry{T}" /> populated with the provided
    ///     registrants.
    /// </summary>
    /// <param name="registrants">The list of objects to register within the registry.</param>
    /// <returns>
    ///     A fully initialized <see cref="FrozenRegistry{T}" /> containing all the provided
    ///     registrants.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="registrants" /> argument is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="registrants" /> list is empty.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when a duplicate ID is found among the
    ///     registrants.
    /// </exception>
    /// <remarks>
    ///     This method ensures that the registry is immutable by using
    ///     <see cref="ImmutableList{T}" /> and <see cref="ImmutableDictionary{TKey,TValue}" />. If the
    ///     same ID appears more than once in the provided list, an
    ///     <see cref="InvalidOperationException" /> will be thrown, as each registrant must have a unique
    ///     identifier.
    /// </remarks>
    [PublicAPI]
    public static FrozenRegistry<T> CreateInstance(IList<T> registrants)
    {
        if (registrants == null)
        {
            throw new ArgumentNullException(nameof(registrants));
        }

        if (registrants.Count == 0)
        {
            throw new ArgumentException("Registry cannot be empty.", nameof(registrants));
        }

        var instance = new FrozenRegistry<T> { _registrants = ImmutableList.CreateRange(registrants), _registrantsKeyed = registrants.ToImmutableDictionary(r => r.Id) };
        var keyedRegistrants = new Dictionary<string, T>();

        for (var i = 0; i < registrants.Count; i++)
        {
            T registrant = registrants[i];

            if (keyedRegistrants.ContainsKey(registrant.Id))
            {
                throw new InvalidOperationException($"An entry with the id '{registrant.Id} is already registred.");
            }

            keyedRegistrants.Add(registrant.Id, registrant);
        }

        return instance;
    }
}
