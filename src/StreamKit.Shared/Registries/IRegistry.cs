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
using System.Diagnostics.CodeAnalysis;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Shared.Registries;

/// <summary>Represents a read-only registry that allows querying objects by their unique identifiers.</summary>
/// <typeparam name="T">
///     The type of object stored in the registry, constrained to implement
///     <see cref="IIdentifiable" />.
/// </typeparam>
/// <remarks>
///     A read-only registry provides access to a fixed collection of objects. Modifications, such
///     as adding or removing objects, are not supported.
/// </remarks>
public interface IReadOnlyRegistry<out T> where T : class, IIdentifiable
{
    /// <summary>Gets all the registered objects in the registry.</summary>
    /// <returns>A list containing all the registered objects.</returns>
    IReadOnlyList<T> AllRegistrants { get; }

    /// <summary>Retrieves an object by its unique identifier.</summary>
    /// <param name="id">The ID of the object to retrieve.</param>
    /// <returns>
    ///     The registered object with the given ID, or <see langword="null" /> if no such object is
    ///     found.
    /// </returns>
    T? Get(string id);
}

/// <summary>Represents a modifiable registry for storing and managing uniquely identifiable objects.</summary>
/// <typeparam name="T">
///     The type of object stored in the registry, constrained to implement
///     <see cref="IIdentifiable" />.
/// </typeparam>
/// <remarks>
///     Registries act as centralized stores for managing collections of objects that can be
///     identified by unique keys (IDs). This interface allows adding, removing, and retrieving
///     registered objects.
/// </remarks>
public interface IRegistry<T> : IReadOnlyRegistry<T> where T : class, IIdentifiable
{
    /// <summary>Registers an object in the registry.</summary>
    /// <param name="obj">The object to register.</param>
    /// <returns>
    ///     <see langword="true" /> if the object was successfully registered; otherwise,
    ///     <see langword="false" /> if it was already registered.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="obj" /> is
    ///     <see langword="null" />.
    /// </exception>
    bool Register([DisallowNull] T obj);

    /// <summary>Unregisters an object from the registry.</summary>
    /// <param name="obj">The object to remove.</param>
    /// <returns>
    ///     <see langword="true" /> if the object was successfully removed; otherwise,
    ///     <see langword="false" /> if the object was not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="obj" /> is
    ///     <see langword="null" />.
    /// </exception>
    bool Unregister([DisallowNull] T obj);
}
