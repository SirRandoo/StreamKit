﻿// MIT License
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
using System.Threading.Tasks;
using StreamKit.Platform.Abstractions;
using WatsonWebsocket;

namespace StreamKit.Platform.Twitch.EventSub
{
    public class TwitchEventSub : IConnection<IResponse>
    {
        private const string EventSubUrl = "wss://eventsub.wss.twitch.tv/ws";
        private static readonly Uri EventSubUri = new Uri(EventSubUrl);
        private WatsonWsClient _primarySocket = new WatsonWsClient(EventSubUri);
        private WatsonWsClient _intermediateSocket = new WatsonWsClient(EventSubUri);

        /// <inheritdoc />
        public bool Connected { get; }

        /// <inheritdoc />
        public ConnectionState State { get; }

        /// <inheritdoc />
        public async Task<bool> ConnectAsync() => throw new NotImplementedException();
        /// <inheritdoc />
        public async Task<bool> DisconnectAsync() => throw new NotImplementedException();
        /// <inheritdoc />
        public async Task<bool> SendAsync(IResponse payload) => throw new NotImplementedException();
    }
}
