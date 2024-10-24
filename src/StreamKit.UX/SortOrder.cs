﻿// MIT License
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

using System;
using NetEscapades.EnumGenerators;

namespace StreamKit.UX;

/// <summary>
///     The various orders data can be ordered.
/// </summary>
[EnumExtensions]
public enum SortOrder
{
    /// <summary>
    ///     The given data has no sort order.
    /// </summary>
    None,

    /// <summary>
    ///     The given data is sorted in ascending order by a given value.
    /// </summary>
    Ascending,

    /// <summary>
    ///     The given data is sorted in descending order by a given value.
    /// </summary>
    Descending
}

/// <summary>
///     A collection of more extension methods for <see cref="SortOrder" />.
/// </summary>
/// <remarks>
///     This class houses extension methods that aren't automatically generated by the
///     <see cref="EnumExtensionsAttribute" />'s associated source generator.
/// </remarks>
public static partial class SortOrderExtensions
{
    /// <summary>
    ///     Returns the inverse of the current sort order.
    /// </summary>
    /// <param name="order">The current sort order being used by a user interface.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Raised when the sort order given isn't either <see cref="SortOrder.None" />,
    ///     <see cref="SortOrder.Ascending" />, or <see cref="SortOrder.Descending" />.
    /// </exception>
    /// <remarks>
    ///     This method is used to flip a user interface element's sort order between
    ///     <see cref="SortOrder.Ascending" /> and <see cref="SortOrder.Descending" />. Should the order be
    ///     <see cref="SortOrder.None" />, the method will instead return
    ///     <see cref="SortOrder.Ascending" />.
    /// </remarks>
    public static SortOrder Invert(this SortOrder order)
    {
        return order switch
        {
            SortOrder.None => SortOrder.Descending,
            SortOrder.Ascending => SortOrder.Descending,
            SortOrder.Descending => SortOrder.Ascending,
            var _ => throw new ArgumentOutOfRangeException(nameof(order), $"An unsupported sort order of '{order.ToString()}' was specified.")
        };
    }
}
