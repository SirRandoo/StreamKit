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

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace StreamKit.Api;

/// <summary>
///     A registry implementation that uses
///     <see
///         cref="Interlocked.CompareExchange(ref object, object, object)"/>
///     to modify the registry's contents.
/// </summary>
/// <typeparam name="T">
///     The type of the class being represented within the registry.
/// </typeparam>
public class SynchronisedRegistry<T> : IRegistry<T> where T : class, IIdentifiable
{
    /// <summary>
    ///     Returns a list of to the objects within the registry.
    /// </summary>
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public ImmutableList<T> AllRegistrants { get; private set; } = ImmutableList<T>.Empty;

    /// <summary>
    ///     Registers an object to the registry.
    /// </summary>
    /// <param name="obj">The object to register</param>
    public void Register([NotNull] T obj)
    {
        ImmutableList<T> original, modified;

        do
        {
            original = AllRegistrants;
            modified = original.Add(obj);
        } while (Interlocked.CompareExchange(ref original, modified, original) != original);
    }

    /// <summary>
    ///     Unregisters an object from the registry.
    /// </summary>
    /// <param name="obj">The object to unregister.</param>
    /// <returns>Whether the object was unregistered.</returns>
    public bool Unregister(T obj)
    {
        ImmutableList<T> original, modified;

        do
        {
            original = AllRegistrants;
            modified = original.Remove(obj);
        } while (Interlocked.CompareExchange(ref original, modified, original) != original);

        return true;
    }
}
