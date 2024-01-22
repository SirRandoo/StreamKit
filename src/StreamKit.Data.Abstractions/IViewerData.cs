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
using System.Collections.Generic;

namespace StreamKit.Data.Abstractions;

/// <summary>
///     Represents the mod data for a given viewer that interacts with
///     the mod.
/// </summary>
public interface IViewerData : IIdentifiable
{
    /// <summary>
    ///     The id of the platform this viewer's data originated from.
    /// </summary>
    string Platform { get; set; }

    /// <summary>
    ///     The amount of points the viewer has accumulated.
    ///     <br/>
    ///     <br/>
    ///     Points are used by the mod's store for purchases, and in some
    ///     other systems for other use cases. For the full rundown of what
    ///     points are used for, please refer to the relevant documentation
    ///     on the mod, and any addons that directly (or indirectly) use
    ///     points.
    /// </summary>
    long Points { get; set; }

    /// <summary>
    ///     The amount of karma the viewer has.
    ///     <br/>
    ///     <br/>
    ///     Karma is the immediate system for altering the buying power of
    ///     viewers. As the karma and reputation systems are relatively
    ///     complex, please refer to the documentation for both in the mod,
    ///     as well as any addons that may directly (or indirectly) alter
    ///     either system.
    /// </summary>
    short Karma { get; set; }

    /// <summary>
    ///     The date and time when the viewer was last seen by the mod.
    ///     <br/>
    ///     <br/>
    ///     How this field is updated depends on the implementation of the
    ///     mod at a given point in time, but it generally refers to the last
    ///     time the viewer was last seen chatting.
    /// </summary>
    DateTime LastSeen { get; set; }

    /// <summary>
    ///     The purchases the viewer has made while using the mod.
    /// </summary>
    /// <remarks>
    ///     The purchase history of a viewer does not always indicate their
    ///     full history as streamers are permitted to clear their history at
    ///     any moment.
    /// </remarks>
    List<ITransaction> Transactions { get; set; }
}
