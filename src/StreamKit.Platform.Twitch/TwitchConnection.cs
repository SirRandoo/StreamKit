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
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SirRandoo.CommonLib.Entities;
using SirRandoo.CommonLib.Interfaces;
using StreamKit.Platform.Abstractions;
using WatsonWebsocket;

namespace StreamKit.Platform.Twitch;

public class TwitchConnection : IConnection<IResponse>, IPlatformSided
{
    private const string TwitchIrcServerUrl = "wss://irc-ws.chat.twitch.tv:443";
    private const string ConnectionId = "Platforms.Twitch.Connection";
    private static readonly IRimLogger Logger = new RimThreadedLogger("StreamKit.Connections.Twitch");
    private static readonly Uri TwitchIrcServerUri = new(TwitchIrcServerUrl);
    private readonly WatsonWsClient _socket = new(TwitchIrcServerUri);

    public TwitchConnection(IPlatform platform)
    {
        Platform = platform;

        _socket.ServerConnected += Authenticate;
        _socket.ServerDisconnected += HandleReconnect;
        _socket.MessageReceived += HandleMessage;
    }

    /// <inheritdoc/>
    public ConnectionState State { get; private set; } = ConnectionState.Disconnected;

    /// <inheritdoc/>
    public bool Connected => _socket.Connected;

    /// <inheritdoc/>
    public async Task<bool> ConnectAsync()
    {
        if (string.IsNullOrEmpty(Platform.Settings.Channel))
        {
            Logger.Error("Tried connecting without a channel set. Set a channel to connect to in the settings menu for StreamKit's Twitch integration.");

            return false;
        }

        try
        {
            State = ConnectionState.Connecting;

            await _socket.StartWithTimeoutAsync();

            State = ConnectionState.Connected;

            await _socket.SendAsync("CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands");

            return true;
        }
        catch (TimeoutException)
        {
            Logger.Error("Could not connect to Twitch -- Connection timed out.");

            State = ConnectionState.Disconnected;

            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DisconnectAsync()
    {
        try
        {
            State = ConnectionState.Disconnecting;

            await _socket.StopAsync();

            State = ConnectionState.Disconnected;

            return true;
        }
        catch (Exception)
        {
            State = ConnectionState.Unknown;

            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> SendAsync([NotNull] IResponse message) => await _socket.SendAsync(message.ToString());

    /// <inheritdoc/>
    [NotNull]
    public string Id => ConnectionId;

    /// <inheritdoc/>
    public IPlatform Platform { get; }

    private async void HandleMessage(object sender, [NotNull] MessageReceivedEventArgs eventArgs)
    {
        if (eventArgs.Data.Count <= 0 || eventArgs.Data.Array == null)
        {
            return;
        }

        try
        {
            string stringified = Encoding.UTF8.GetString(eventArgs.Data.Array);
        }
        catch (ArgumentException)
        {
            // Unused.
            // We don't care about improperly formatted unicode points, so we'll
            // just skip any messages that couldn't be decoded into a string.
        }
    }

    private async void HandleReconnect(object sender, EventArgs eventArgs)
    {
        await TryReconnect();
    }

    private async Task<bool> TryReconnect()
    {
        var triesLeft = 5;

        while (triesLeft > 0)
        {
            if (await ConnectAsync())
            {
                return true;
            }

            await Task.Delay(2 * (5 - triesLeft) + 6);
            triesLeft--;
        }

        return false;
    }

    private async void Authenticate(object sender, EventArgs eventArgs)
    {
        State = ConnectionState.Authenticating;

        if (string.IsNullOrEmpty(Platform.Settings.Token))
        {
            await _socket.SendAsync("PASS foobar");
            await _socket.SendAsync("NICK justinfan920");
        }
        else
        {
            await _socket.SendAsync($"PASS {Platform.Settings.Token}");
            await _socket.SendAsync($"NICK {Platform.Settings.User}");
        }

        State = ConnectionState.Authenticated;


        State = ConnectionState.Joining;

        await _socket.SendAsync($"JOIN #{Platform.Settings.Channel}");

        State = ConnectionState.Joined;
    }

    public async Task<bool> SendRawMessage(string ircMessage) => await _socket.SendAsync(ircMessage);

    public async Task<bool> SendPrivateIrcMessage(string message) => await SendRawMessage($"PRIVMSG #{Platform.Settings.Channel} :{message}");
}
