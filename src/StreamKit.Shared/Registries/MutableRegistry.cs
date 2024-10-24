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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Shared.Registries;

/// <summary>
///     A simple mutable registry for managing unique objects that implement the
///     <see cref="IIdentifiable" /> interface. This class allows for adding, removing, and retrieving
///     objects using their unique identifiers.
/// </summary>
/// <typeparam name="T">
///     The type of objects stored in the registry. Must implement
///     <see cref="IIdentifiable" />.
/// </typeparam>
[PublicAPI]
public class MutableRegistry<T> : IRegistry<T> where T : class, IIdentifiable
{
    private Dictionary<string, T> _allRegistrantsKeyed = null!;
    private IReadOnlyList<T>? _cachedRegistrantView;

    private MutableRegistry()
    {
    }

    /// <summary>
    ///     Gets the current list of all registered objects as a read-only view. The list is cached
    ///     for performance and will be updated if any modifications occur.
    /// </summary>
    public IReadOnlyList<T> AllRegistrants => _cachedRegistrantView ??= _allRegistrantsKeyed.Values.ToList();

    /// <summary>
    ///     Registers a new object in the registry. If an object with the same identifier already
    ///     exists, the registration will fail.
    /// </summary>
    /// <param name="obj">The object to register.</param>
    /// <returns>
    ///     <see langword="true" /> if the object was successfully registered; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool Register([DisallowNull] T obj)
    {
        if (_allRegistrantsKeyed.ContainsKey(obj.Id))
        {
            return false;
        }

        _allRegistrantsKeyed[obj.Id] = obj;
        _cachedRegistrantView = null;

        return true;
    }

    /// <summary>
    ///     Unregisters an existing object from the registry. If the object does not exist in the
    ///     registry, the operation will fail.
    /// </summary>
    /// <param name="obj">The object to unregister.</param>
    /// <returns>
    ///     <see langword="true" /> if the object was successfully unregistered; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool Unregister([DisallowNull] T obj)
    {
        if (!_allRegistrantsKeyed.Remove(obj.Id))
        {
            return false;
        }

        _cachedRegistrantView = null;

        return true;
    }

    /// <summary>
    ///     Retrieves an object from the registry using its unique identifier. Returns
    ///     <see langword="null" /> if the object does not exist.
    /// </summary>
    /// <param name="id">The unique identifier of the object to retrieve.</param>
    /// <returns>
    ///     The object associated with the specified identifier, or <see langword="null" /> if not
    ///     found.
    /// </returns>
    public T? Get(string id) => _allRegistrantsKeyed.TryGetValue(id, out T? value) ? value : default;

    /// <summary>
    ///     Creates an instance of <see cref="MutableRegistry{T}" /> and populates it with initial
    ///     registrants. This method checks for duplicate identifiers in the initial list and throws an
    ///     exception if found.
    /// </summary>
    /// <param name="registrants">The initial list of objects to register.</param>
    /// <returns>
    ///     A new instance of <see cref="MutableRegistry{T}" /> populated with the provided
    ///     registrants.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a duplicate id is detected in the initial
    ///     registrants.
    /// </exception>
    public static MutableRegistry<T> CreateInstance(IReadOnlyList<T> registrants)
    {
        var registrantsKeyed = new Dictionary<string, T>();

        for (var i = 0; i < registrants.Count; i++)
        {
            T registrant = registrants[i];

            if (registrantsKeyed.ContainsKey(registrant.Id))
            {
                throw new InvalidOperationException($"An entry with the id '{registrant.Id}' is already registered.");
            }

            registrantsKeyed.Add(registrant.Id, registrant);
        }

        return new MutableRegistry<T> { _allRegistrantsKeyed = registrantsKeyed };
    }
}
