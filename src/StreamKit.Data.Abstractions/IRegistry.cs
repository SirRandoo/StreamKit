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
using System.Diagnostics.CodeAnalysis;

namespace StreamKit.Data.Abstractions;

/// <summary>
///     An abstract definition of a "registry", an object designated for
///     centralized storing and retrieving unique objects.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRegistry<T> where T : class, IIdentifiable
{
    /// <summary>
    ///     Returns the current objects registered within the registry.
    /// </summary>
    IList<T> AllRegistrants { get; }

    /// <summary>
    ///     Registers an object to the registry.
    /// </summary>
    /// <param name="obj">The object to register</param>
    /// <returns>Whether the object was registered.</returns>
    bool Register([NotNull] T obj);

    /// <summary>
    ///     Unregisters an object from the registry.
    /// </summary>
    /// <param name="obj">The object to unregister.</param>
    /// <returns>Whether the object was unregistered.</returns>
    bool Unregister(T obj);

    /// <summary>
    ///     Gets the object within the registry by the id, or
    ///     <see langword="null"/> if the object doesn't exist.
    /// </summary>
    /// <param name="id">The id of the object being obtained.</param>
    /// <returns>
    ///     The object registered within the registry, or the
    ///     <see langword="null"/> if the object doesn't exist.
    /// </returns>
    T? Get(string id);
}
