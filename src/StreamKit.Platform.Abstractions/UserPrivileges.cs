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

using System;
using NetEscapades.EnumGenerators;

namespace StreamKit.Platform.Abstractions
{
    /// <summary>
    ///     The various types of users that can exist within the mod.
    /// </summary>
    [Flags]
    [EnumExtensions]
    public enum UserPrivileges
    {
        /// <summary>
        ///     The user has no types, and should be considered a basic user.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The user is a follower of the given channel.
        /// </summary>
        Follower = 1,

        /// <summary>
        ///     The user is a premium user whether it be through the platform the
        ///     user is on, or an external subscription-based platform, like
        ///     Patreon.
        /// </summary>
        Premium = 2,

        /// <summary>
        ///     The user is contains permissions only available to channel
        ///     moderators.
        /// </summary>
        Moderation = 4
    }
}
