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

using System;

namespace StreamKit.Common.Data.Abstractions;

/// <summary>
///     Represents a user's transaction.
/// </summary>
public interface ITransaction : IIdentifiable
{
    /// <summary>
    ///     The amount of points that the user paid at the point in time.
    /// </summary>
    long Amount { get; set; }

    /// <summary>
    ///     Whether the transaction was refunded to the user.
    /// </summary>
    bool Refunded { get; set; }

    /// <summary>
    ///     The type of product that was purchased at the given point in time.
    /// </summary>
    string ProductId { get; set; }

    /// <summary>
    ///     The morality of the transaction at the time it was made.
    /// </summary>
    Morality Morality { get; set; }

    /// <summary>
    ///     The date/time the transaction occurred at.
    /// </summary>
    DateTime OccurredAt { get; set; }
}
