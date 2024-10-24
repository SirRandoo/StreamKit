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

namespace StreamKit.Shared.Interfaces;

/// <summary>
///     Represents a data object housing the ratelimit data for a product, command, or other
///     unspecified ratelimited resource.
/// </summary>
public interface IRateLimitData
{
    /// <summary>
    ///     Represents the amount of time a viewer has to wait before they'll be allowed to use the
    ///     associated command, or purchase the associated product.
    /// </summary>
    TimeSpan? LocalCooldown { get; set; }

    /// <summary>
    ///     Represents the amount of time all viewers has to wait before they'll be allowed to use the
    ///     associated command, or purchase the associated product.
    /// </summary>
    TimeSpan? GlobalCooldown { get; set; }
}
