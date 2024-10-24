// MIT License
//
// Copyright (c) 2024 SirRandoo
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

using Verse;

namespace StreamKit.UX.Extensions;

/// <summary>
///     A collection of extension methods for strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     An alternative to <see cref="Gen.ToStringSafe{T}" /> that returns <see cref="string.Empty" />
    ///     instead of "null".
    /// </summary>
    /// <param name="instance">An instance of an object that's being turned into a string.</param>
    /// <typeparam name="T">The type of the object being turned into a string.</typeparam>
    /// <returns>
    ///     <see cref="string.Empty" /> if the object was <see langword="null" />, or the object's string
    ///     representation.
    /// </returns>
    public static string ToStringNullable<T>(this T? instance) => instance == null ? string.Empty : instance.ToString();
}
