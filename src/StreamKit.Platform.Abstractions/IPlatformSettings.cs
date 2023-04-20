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

using System.ComponentModel;
using StreamKit.Api;

namespace StreamKit.Platform.Abstractions
{
    /// <summary>
    ///     A derivative of <see cref="IComponentSettings"/> that expresses
    ///     generic data that may be useful to <see cref="IPlatform"/> and
    ///     <see cref="IConnection"/> implementations such as an OAuth2 token
    ///     property, the username of the account being used for the
    ///     connection, and the channel to join when the connection is fully
    ///     established.
    /// </summary>
    public interface IPlatformSettings : IComponentSettings
    {
        /// <summary>
        ///     A unique string of characters for use when authenticating to a
        ///     platform when the <see cref="IConnection"/> enters is
        ///     authentication stage.
        /// </summary>
        string Token { get; set; }

        /// <summary>
        ///     The username, or id if applicable, of the channel to connect to
        ///     when the <see cref="IConnection"/> enters its authentication
        ///     stage.
        /// </summary>
        string Channel { get; set; }

        /// <summary>
        ///     The username, or id if applicable, of the account to use when
        ///     during the authentication stage of a <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/>.
        /// </summary>
        string User { get; set; }

        /// <summary>
        ///     Indicates whether the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> should automatically connect to the
        ///     platform's service when the game loads.
        /// </summary>
        /// <remarks>
        ///     This is typically done in the <see cref="Verse.Mod"/>'s
        ///     constructor, or in a class's static constructor with the class
        ///     decorated with the <see cref="Verse.StaticConstructorOnStartup"/>
        ///     attribute.
        /// </remarks>
        [DefaultValue(false)]
        bool AutoConnect { get; set; }

        /// <summary>
        ///     Indicates whether the <see cref="IConnection"/> should send a
        ///     "connected" message when it finishes connecting to the channel.
        /// </summary>
        [DefaultValue(false)]
        bool ConnectionMessage { get; set; }
    }
}
