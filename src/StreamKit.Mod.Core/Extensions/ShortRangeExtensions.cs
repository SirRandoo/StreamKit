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

using JetBrains.Annotations;
using StreamKit.Shared;
using Verse;

namespace StreamKit.Mod.Core.Extensions;

/// <summary>
///     A collection of extension methods for modifying <see cref="ShortRange" />s without an
///     allocation.
/// </summary>
/// <remarks>
///     These extension methods mutate an existing <see cref="ShortRange" />, and thus aren't suitable
///     for operations that require immutable modifications.
/// </remarks>
[PublicAPI]
public static class ShortRangeExtensions
{
    /// <summary>
    ///     Returns a random value between the upper and lower bounds of the range.
    /// </summary>
    /// <param name="range">
    ///     A <see cref="ShortRange" /> instance indicating the upper and lower bounds a value can be in.
    /// </param>
    public static short GetRandomValue(this ShortRange range) => (short)Rand.Range(range.Minimum, range.Maximum);
}
