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

using NetEscapades.EnumGenerators;

namespace StreamKit.Platform.Abstractions
{
    /// <summary>
    ///     An enumeration used to indicate the current state of a
    ///     <see cref="IPlatform"/>'s underlying <see cref="IConnection"/>.
    ///     <br/>
    ///     <br/>
    ///     Connection states are used by the mod to display connection
    ///     information to the user in the platform's relevant connection
    ///     menu.
    /// </summary>
    [EnumExtensions]
    public enum ConnectionState
    {
        /// <summary>
        ///     Used to indicate that the <see cref="IConnection"/> could not
        ///     properly determine its current state.
        /// </summary>
        /// <remarks>
        ///     This value should only be used when the <see cref="IConnection"/>
        ///     experiences a problem it can't properly recover from -- leaving
        ///     it in an unusable state.
        /// </remarks>
        Unknown,

        /// <summary>
        ///     Used to indicate that the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> isn't currently connected to the
        ///     service.
        /// </summary>
        Disconnected,

        /// <summary>
        ///     Used to indicate that the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> is in the process of disconnecting from
        ///     the service.
        /// </summary>
        Disconnecting,

        /// <summary>
        ///     Used to indicate that the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> is currently connected to the service,
        ///     and may also indicate that it's ready for authentication.
        /// </summary>
        Connected,

        /// <summary>
        ///     Used to indicate that the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> is currently connecting to the service.
        /// </summary>
        Connecting,

        /// <summary>
        ///     Used to indicate that the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> is currently reconnecting to the
        ///     service.
        /// </summary>
        /// <remarks>
        ///     Generally this is only used when the <see cref="IConnection"/>
        ///     receives a reconnect request from the service, or the connection
        ///     itself encountered a problem it couldn't recover from. From
        ///     there, this value is quickly replaced by <see cref="Connecting"/>
        ///     or <see cref="Disconnected"/> as the connection goes through the
        ///     normal flow.
        /// </remarks>
        Reconnecting,

        /// <summary>
        ///     Used to indicate that the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> is in the process of authenticating to
        ///     the service with the credentials provided.
        /// </summary>
        Authenticating,

        /// <summary>
        ///     Used to indicate that the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> successfully authenticated with the
        ///     service with the provided credentials. This stage may
        ///     additionally indicate that the connection is considered "ready
        ///     for use."
        /// </summary>
        Authenticated,

        /// <summary>
        ///     Used to indicate that the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> successfully joined the channel
        ///     specified. This stage will always indicate that the connection is
        ///     considered "ready for use."
        /// </summary>
        Joined,

        /// <summary>
        ///     Used to indicate that the <see cref="IPlatform"/>'s
        ///     <see cref="IConnection"/> is in the process of joining the
        ///     channel specified by the user.
        /// </summary>
        Joining
    }
}
