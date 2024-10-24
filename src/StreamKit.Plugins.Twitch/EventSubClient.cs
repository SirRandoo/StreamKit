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
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using vtortola.WebSockets.Rfc6455;

namespace StreamKit.Plugins.Twitch;

public class EventSubClient
{
    private WebSocket? _webSocket;
    private readonly WebSocketClient _webSocketClient;

    public EventSubClient()
    {
        const int bufferSize = 1024 * 8;
        const int bufferPoolSize = 100 * bufferSize;

        var websocketOptions = new WebSocketListenerOptions { SendBufferSize = bufferSize, BufferManager = BufferManager.CreateBufferManager(bufferPoolSize, bufferSize) };

        websocketOptions.Standards.RegisterRfc6455();

        websocketOptions.Transports.ConfigureTcp(
            tcp =>
            {
                tcp.BacklogSize = 100;
                tcp.ReceiveBufferSize = bufferSize;
                tcp.SendBufferSize = bufferSize;
            }
        );

        _webSocketClient = new WebSocketClient(websocketOptions);
    }

    public async Task ConnectAsync(string url = "wss://eventsub.wss.twitch.tv/ws", CancellationToken ct = default)
    {
        if (_webSocket != null)
        {
            await _webSocket.CloseAsync();
            await _webSocketClient.CloseAsync();
        }

        _webSocket = await _webSocketClient.ConnectAsync(new Uri(url), ct);
    }

    public async Task DisconnectAsync()
    {
        if (_webSocket != null)
        {
            await _webSocket.CloseAsync();
        }

        await _webSocketClient.CloseAsync();
    }
}

internal sealed class EventSubMessageMetadata
{
    [JsonPropertyName("message_id")] public string MessageId { get; init; } = null!;
    [JsonPropertyName("message_type")] public string MessageType { get; init; } = null!;
    [JsonPropertyName("message_timestamp")] public DateTime MessageBody { get; init; }
}

internal abstract class EventSubMessage<T> where T : class
{
    [JsonPropertyName("metadata")] public EventSubMessageMetadata Metadata { get; init; } = null!;
    [JsonPropertyName("payload")] public T Payload { get; init; } = null!;
}

internal sealed class WelcomeMessage : EventSubMessage<WelcomeMessage.SessionPayload>
{
    internal sealed class SessionPayload
    {
        [JsonPropertyName("session")] public Session Session { get; init; } = null!;
    }

    internal sealed class Session
    {
        [JsonPropertyName("id")] public string Id { get; init; } = null!;
        [JsonPropertyName("status")] public string Status { get; init; } = null!;
        [JsonPropertyName("connected_at")] public DateTime ConnectedAt { get; init; }
        [JsonPropertyName("keepalive_timeout_seconds")] public int KeepAliveTimeout { get; init; }
        [JsonPropertyName("reconnect_url")] public string? ReconnectUrl { get; init; }
    }
}

internal sealed class KeepAliveMessage : EventSubMessage<KeepAliveMessage.KeepAlivePayload>
{
    internal sealed class KeepAlivePayload {}
}

internal sealed class NotificationMessage : EventSubMessage<NotificationMessage.NotificationPayload>
{
    internal sealed class NotificationPayload
    {
        [JsonPropertyName("subscription")] public Subscription Subscription { get; init; } = null!;
        [JsonPropertyName("event")] public Event Event { get; init; } = null!;
    }

    internal sealed class Event
    {
        [JsonPropertyName("user_id")] public string UserId { get; init; } = null!;
        [JsonPropertyName("user_login")] public string Login { get; init; } = null!;
        [JsonPropertyName("user_name")] public string UserName { get; init; } = null!;
        [JsonPropertyName("broadcaster_user_id")] public string BroadcasterUserId { get; init; } = null!;
        [JsonPropertyName("broadcaster_user_login")] public string BroadcasterUserLogin { get; init; } = null!;
        [JsonPropertyName("broadcaster_user_name")] public string BroadcasterUserName { get; init; } = null!;
        [JsonPropertyName("followed_at")] public DateTime FollowedAt { get; init; }
    }

    internal sealed class Subscription
    {
        [JsonPropertyName("id")] public string Id { get; init; } = null!;
        [JsonPropertyName("status")] public string Status { get; init; } = null!;
        [JsonPropertyName("type")] public string Type { get; init; } = null!;
        [JsonPropertyName("version")] public string Version { get; init; } = null!;
        [JsonPropertyName("cost")] public int Cost { get; init; }
        [JsonPropertyName("condition")] public Condition Condition { get; init; } = null!;
        [JsonPropertyName("transport")] public Transport Transport { get; init; } = null!;
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; init; }
    }

    internal sealed class Condition
    {
        [JsonPropertyName("broadcaster_user_id")] public string BroadcasterUserId { get; init; } = null!;
    }

    internal sealed class Transport
    {
        [JsonPropertyName("method")] public string Method { get; init; } = null!;
        [JsonPropertyName("session_id")] public string SessionId { get; init; } = null!;
    }
}

internal sealed class ReconnectMessage : EventSubMessage<ReconnectMessage.SessionPayload>
{
    internal sealed class SessionPayload
    {
        [JsonPropertyName("id")] public string Id { get; init; } = null!;
        [JsonPropertyName("status")] public string Status { get; init; } = null!;
        [JsonPropertyName("keepalive_timeout_seconds")] public int? KeepAliveTimeout { get; init; }
        [JsonPropertyName("reconnect_url")] public string ReconnectUrl { get; init; } = null!;
        [JsonPropertyName("connected_at")] public DateTime ConnectedAt { get; init; }
    }
}

internal sealed class RevocationMessage : EventSubMessage<RevocationMessage.Subscription>
{
    internal sealed class Subscription
    {
        [JsonPropertyName("id")] public string Id { get; init; } = null!;
        [JsonPropertyName("status")] public string Status { get; init; } = null!;
        [JsonPropertyName("type")] public string Type { get; init; } = null!;
        [JsonPropertyName("version")] public string Version { get; init; } = null!;
        [JsonPropertyName("cost")] public int Cost { get; init; }
        [JsonPropertyName("condition")] public Condition Condition { get; init; } = null!;
        [JsonPropertyName("transport")] public Transport Transport { get; init; } = null!;
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; init; }
    }

    internal sealed class Condition
    {
        [JsonPropertyName("broadcaster_user_id")] public string BroadcasterUserId { get; init; } = null!;
    }

    internal sealed class Transport
    {
        [JsonPropertyName("method")] public string Method { get; init; } = null!;
        [JsonPropertyName("session_id")] public string SessionId { get; init; } = null!;
    }
}
