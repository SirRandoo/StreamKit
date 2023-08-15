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

namespace StreamKit.Platform.Twitch.EventSub;

/// <summary>
///     An interface representing the session information sent by the
///     EventSub system.
/// </summary>
public interface IEventSubSession
{
    /// <summary>
    ///     An ID that uniquely identifies this WebSocket connection. Use
    ///     this ID to set the session id field in all subscription requests.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    ///     The current status of the EventSub connection.
    /// </summary>
    public EventConnectionStatus Status { get; set; }

    /// <summary>
    ///     The maximum number of seconds that you should expect silence
    ///     before receiving a keepalive message. For a welcome message, this
    ///     is the number of seconds that you have to subscribe to an event
    ///     after receiving the welcome message. If you don’t subscribe to an
    ///     event within this window, the socket is disconnected.
    /// </summary>
    public int KeepAliveTimeoutSeconds { get; set; }

    /// <summary>
    ///     The URL to reconnect to if you get a Reconnect message.
    /// </summary>
    public Uri ReconnectUrl { get; set; }

    /// <summary>
    ///     The UTC date and time that the connection was created.
    /// </summary>
    public DateTime ConnectedAt { get; set; }
}
