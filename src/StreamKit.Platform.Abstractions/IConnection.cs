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

using System.Threading.Tasks;

namespace StreamKit.Platform.Abstractions
{
    /// <summary>
    ///     An interface for outlining a connection to a
    ///     <see cref="IPlatform"/>'s service.
    /// </summary>
    public interface IConnection : IPlatformSided
    {
        /// <summary>
        ///     The current state of the connection.
        /// </summary>
        ConnectionState State { get; }

        /// <summary>
        ///     Returns whether or not the underlying connection being
        ///     represented is connected to the platform's servers.
        /// </summary>
        bool Connected { get; }

        /// <summary>
        ///     Connects to the <see cref="IPlatform"/>'s service.
        /// </summary>
        /// <returns>
        ///     Whether a connection could be established to the
        ///     <see cref="IPlatform"/>'s service.
        /// </returns>
        Task<bool> Connect();

        /// <summary>
        ///     Disconnects from the <see cref="IPlatform"/>'s service.
        /// </summary>
        /// <returns>
        ///     Whether the connection to the <see cref="IPlatform"/>'s
        ///     service was successfully closed.
        /// </returns>
        Task<bool> Disconnect();

        /// <summary>
        ///     Sends a message to the platform.
        /// </summary>
        /// <param name="message">The message to send to the platform.</param>
        /// <returns>Whether the message was successfully sent.</returns>
        Task<bool> SendMessage(IResponse message);
    }
}
