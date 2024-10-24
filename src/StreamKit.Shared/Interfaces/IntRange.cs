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

namespace StreamKit.Shared.Interfaces;

/// <summary>
///     A specialized struct representing an upper and lower bound within a <see cref="int" />'s
///     minimum and maximum values.
/// </summary>
/// <param name="minimum">The lower bound of the range.</param>
/// <param name="maximum">The upper bound of the range.</param>
public struct IntRange(int minimum = int.MinValue, int maximum = int.MaxValue)
{
    /// <summary>
    ///     The lower bound of the range.
    /// </summary>
    public int Minimum = minimum;

    /// <summary>
    ///     The upper bound of the range.
    /// </summary>
    public int Maximum = maximum;
}
