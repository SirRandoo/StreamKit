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
using System.Collections.Generic;

namespace StreamKit.Common.Data.Abstractions;

/// <summary>
///     Represents a user on a given platform.
/// </summary>
public interface IUser : IIdentifiable
{
    /// <summary>
    ///     The amount of points a user currently has available.
    /// </summary>
    long Points { get; set; }

    /// <summary>
    ///     The amount of karma the user has accumulated across their purchases.
    /// </summary>
    short Karma { get; set; }

    /// <summary>
    ///     The platform the user was last seen on.
    /// </summary>
    string Platform { get; init; }

    /// <summary>
    ///     The date/time when the user was last seen in chat.
    /// </summary>
    DateTime LastSeen { get; set; }

    /// <summary>
    ///     The user's privileges within the chat.
    /// </summary>
    UserPrivileges Privileges { get; set; }

    /// <summary>
    ///     The list of purchases the user has authorized.
    /// </summary>
    List<ITransaction> Transactions { get; init; }
}
