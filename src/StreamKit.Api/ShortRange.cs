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

using Verse;

namespace StreamKit.Api;

/// <summary>
///     A specialized struct representing an upper and lower bound within a <see cref="short" />'s
///     minimum and maximum values.
/// </summary>
/// <param name="minimum">The lower bound of the range.</param>
/// <param name="maximum">The upper bound of the range.</param>
public struct ShortRange(short minimum = short.MinValue, short maximum = short.MaxValue)
{
    /// <summary>
    ///     The lower bound of the range.
    /// </summary>
    public short Minimum = minimum;

    /// <summary>
    ///     The upper bound of the range.
    /// </summary>
    public short Maximum = maximum;
}

/// <summary>
///     A collection of extension methods for modifying <see cref="ShortRange" />s without an
///     allocation.
/// </summary>
/// <remarks>
///     These extension methods mutate an existing <see cref="ShortRange" />, and thus aren't suitable
///     for operations that require immutable modifications.
/// </remarks>
public static class ShortRangeExtensions
{
    /// <summary>
    ///     Updates the minimum value for a given <see cref="ShortRange" />.
    /// </summary>
    /// <param name="range">The range being updated.</param>
    /// <param name="minimum">The new minimum value of the range.</param>
    /// <returns>The new, mutated, range.</returns>
    public static ref ShortRange SetMinimum(this ref ShortRange range, short minimum)
    {
        range.Minimum = minimum;

        return ref range;
    }

    /// <summary>
    ///     Updates the maximum value for a given <see cref="ShortRange" />.
    /// </summary>
    /// <param name="range">The range being updated.</param>
    /// <param name="maximum">The new maximum value of the range.</param>
    /// <returns>The new, mutated, range.</returns>
    public static ref ShortRange SetMaximum(this ref ShortRange range, short maximum)
    {
        range.Maximum = maximum;

        return ref range;
    }

    /// <summary>
    ///     Returns a random value between the upper and lower bounds of the range.
    /// </summary>
    /// <param name="range">
    ///     A <see cref="ShortRange" /> instance indicating the upper and lower bounds a value can be in.
    /// </param>
    public static short GetRandomValue(this ShortRange range) => (short)Rand.Range(range.Minimum, range.Maximum);
}
