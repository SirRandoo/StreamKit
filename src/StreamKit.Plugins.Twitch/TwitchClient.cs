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

namespace StreamKit.Plugins.Twitch;

// TODO: Implement a generic system for emitting notifications.
//       Notification should include:
//          - Channel raids
//          - Premium currency, and/or irl money, spent
//          - Poll completions
//          - User subscriptions
//          - Gift subscriptions
//          - Chat messages
//          - 3rd party redemptions, like Twitch's channel point rewards.

// public class TwitchClient
// {
//     private const string EventSubBroadcasterIdKey = "broadcaster_user_id";
//
//     private static readonly Logger Logger = KitLogManager.GetLogger("StreamKit.Integrations.Twitch");
//
//     private readonly EventSubWebsocketClient _eventSubClient = new();
//
//     public TwitchClient()
//     {
//         _eventSubClient.ChannelRaid += OnChannelRaid;
//         _eventSubClient.ChannelCheer += OnChannelCheer;
//         _eventSubClient.ChannelPollEnd += OnChannelPollEnd;
//         _eventSubClient.ChannelSubscribe += OnChannelSubscribe;
//         _eventSubClient.ChannelChatMessage += OnChannelChatMessage;
//         _eventSubClient.ChannelSubscriptionGift += OnChannelSubscriptionGift;
//         _eventSubClient.ChannelPointsCustomRewardRedemptionAdd += OnCustomRewardRedemptionAdd;
//
//         _eventSubClient.ErrorOccurred += OnWebsocketErrorOccurred;
//         _eventSubClient.WebsocketConnected += OnWebsocketConnected;
//         _eventSubClient.WebsocketReconnected += OnWebsocketReconnected;
//         _eventSubClient.WebsocketDisconnected += OnWebsocketDisconnected;
//     }
//
//     private async Task OnChannelSubscriptionGift(object sender, ChannelSubscriptionGiftArgs args)
//     {
//         if (ShouldSkipMessage(args.Notification.Metadata))
//         {
//             return;
//         }
//
//         // TODO: Pass the notification on to the mod's api so other areas can process it.
//         throw new NotImplementedException();
//     }
//
//     private async Task OnChannelSubscribe(object sender, ChannelSubscribeArgs args)
//     {
//         if (ShouldSkipMessage(args.Notification.Metadata))
//         {
//             return;
//         }
//
//         // TODO: Pass the notification on to the mod's api so other areas can process it.
//         throw new NotImplementedException();
//     }
//
//     private async Task OnChannelRaid(object sender, ChannelRaidArgs args)
//     {
//         if (ShouldSkipMessage(args.Notification.Metadata))
//         {
//             return;
//         }
//
//         // TODO: Pass the notification on to the mod's api so other areas can process it.
//         throw new NotImplementedException();
//     }
//
//     private async Task OnChannelPollEnd(object sender, ChannelPollEndArgs args)
//     {
//         if (ShouldSkipMessage(args.Notification.Metadata))
//         {
//             return;
//         }
//
//         // TODO: Pass the notification on to the mod's api so other areas can process it.
//         throw new NotImplementedException();
//     }
//
//     private async Task OnCustomRewardRedemptionAdd(object sender, ChannelPointsCustomRewardRedemptionArgs args)
//     {
//         if (ShouldSkipMessage(args.Notification.Metadata))
//         {
//             return;
//         }
//
//         // TODO: Pass the notification on the the mod's api so other areas can process it.
//         throw new NotImplementedException();
//     }
//
//     private async Task OnChannelCheer(object sender, ChannelCheerArgs args)
//     {
//         if (ShouldSkipMessage(args.Notification.Metadata))
//         {
//             return;
//         }
//
//         // TODO: Pass the notification on to the mod's api so other areas can process it.
//         throw new NotImplementedException();
//     }
//
//     private async Task OnChannelChatMessage(object sender, ChannelChatMessageArgs args)
//     {
//         if (ShouldSkipMessage(args.Notification.Metadata))
//         {
//             return;
//         }
//
//         IMessage message = TwitchTranscriber.TranscribeMessage(args.Notification.Payload.Event);
//
//         // TODO: Pass the message to the mod's api so other areas can process it.
//         throw new NotImplementedException();
//     }
//
//     private bool ShouldSkipMessage(EventSubMetadata metadata)
//     {
//         // Since Twitch may resend messages, they advise skipping messages older than 10 minutes,
//         // as well as keeping track of messages the mod previously saw.
//         bool shouldSkip = (DateTime.UtcNow - metadata.MessageTimestamp).TotalMinutes >= 10 || _seenMessageIds.Contains(metadata.MessageId);
//
//         if (!shouldSkip)
//         {
//             _seenMessageIds.Add(metadata.MessageId);
//         }
//
//         return shouldSkip;
//     }
//
//     private static async Task OnWebsocketConnected(object sender, WebsocketConnectedArgs args) => Logger.Info(
//         "Connected to EventSub :: Was due to reconnect notice? {Reconnect}",
//         args.IsRequestedReconnect
//     );
//
//     private static async Task OnWebsocketReconnected(object sender, EventArgs eventArgs) => Logger.Info("Reconnected to EventSub");
//
//     private static async Task OnWebsocketDisconnected(object sender, EventArgs eventArgs) => Logger.Warn("Disconnected from EventSub");
//
//     private static async Task OnWebsocketErrorOccurred(object sender, ErrorOccuredArgs args)
//     {
//         if (args.Exception != null)
//         {
//             Logger.Error(args.Exception, "EventSub connection encountered an error: {Error}", args.Message);
//         }
//         else
//         {
//             Logger.Error("EventSub connection encountered an error: {Error}", args.Message);
//         }
//     }
//
//     public async Task ConnectAsync()
//     {
//         await _eventSubClient.ConnectAsync();
//
//         await SubscribeToPollEnd();
//         await SubscribeToRaidsAsync();
//         await SubscribeToCheersAsync();
//         await SubscribeToSubscriptionsAsync();
//         await SubscribeToChannelMessagesAsync();
//         await SubscribeToGiftSubscriptionsAsync();
//         await SubscribeToCustomRewardRedemptionsAsync();
//     }
//
//     private async Task DisconnectAsync()
//     {
//         await _eventSubClient.DisconnectAsync();
//     }
//
//     private async Task SubscribeToChannelMessagesAsync()
//     {
//         await SubscribeAsync(
//             EventSubChatMessage.Id,
//             EventSubChatMessage.Version,
//             new Dictionary<string, string>
//             {
//                 [EventSubBroadcasterIdKey] = "123", // FIXME: Replace with broadcaster's user id.
//                 ["user_id"] = "321" // FIXME: Replace with the user id for the token given.
//             }
//         );
//     }
//
//     private async Task SubscribeToSubscriptionsAsync()
//     {
//         await SubscribeAsync(
//             EventSubSubscription.Id,
//             EventSubSubscription.Version,
//             new Dictionary<string, string>
//             {
//                 [EventSubBroadcasterIdKey] = "123" // FIXME: Replace with broadcaster's user id.
//             }
//         );
//     }
//
//     private async Task SubscribeToGiftSubscriptionsAsync()
//     {
//         await SubscribeAsync(
//             EventSubSubscriptionGift.Id,
//             EventSubSubscriptionGift.Version,
//             new Dictionary<string, string>
//             {
//                 [EventSubBroadcasterIdKey] = "123" // FIXME: Replace with broadcaster's user id.
//             }
//         );
//     }
//
//     private async Task SubscribeToCheersAsync()
//     {
//         await SubscribeAsync(
//             EventSubChannelCheer.Id,
//             EventSubChannelCheer.Version,
//             new Dictionary<string, string>
//             {
//                 [EventSubBroadcasterIdKey] = "123" // FIXME: Replace with broadcaster's user id.
//             }
//         );
//     }
//
//     private async Task SubscribeToRaidsAsync()
//     {
//         await SubscribeAsync(
//             EventSubRaid.Id,
//             EventSubRaid.Version,
//             new Dictionary<string, string>
//             {
//                 [EventSubBroadcasterIdKey] = "123" // FIXME: Replace with broadcaster's user id.
//             }
//         );
//     }
//
//     private async Task SubscribeToCustomRewardRedemptionsAsync()
//     {
//         await SubscribeAsync(
//             EventSubCustomRewardRedemption.Id,
//             EventSubCustomRewardRedemption.Version,
//             new Dictionary<string, string>
//             {
//                 [EventSubBroadcasterIdKey] = "123" // FIXME: Replace with broadcaster's user id.
//             }
//         );
//     }
//
//     private async Task SubscribeToPollEnd()
//     {
//         await SubscribeAsync(
//             EventSubChannelPollEnd.Id,
//             EventSubChannelPollEnd.Version,
//             new Dictionary<string, string>
//             {
//                 [EventSubBroadcasterIdKey] = "123" // FIXME: Replace with broadcaster's user id.
//             }
//         );
//     }
//
//     private async Task SubscribeAsync(string type, string version, Dictionary<string, string> condition)
//     {
//         // FIXME: Insert token into this call.
//         // await _helixApi.EventSub.CreateEventSubSubscriptionAsync(type, version, condition, EventSubTransportMethod.Websocket, _eventSubClient.SessionId);
//     }
//
//     private static class EventSubChatMessage
//     {
//         public const string Id = "channel.chat.message";
//         public const string Version = "1";
//     }
//
//     private static class EventSubSubscription
//     {
//         public const string Id = "channel.subscribe";
//         public const string Version = "1";
//     }
//
//     private static class EventSubSubscriptionGift
//     {
//         public const string Id = "channel.subscription.gift";
//         public const string Version = "1";
//     }
//
//     private static class EventSubChannelPollEnd
//     {
//         public const string Id = "channel.poll.end";
//         public const string Version = "1";
//     }
//
//     private static class EventSubCustomRewardRedemption
//     {
//         public const string Id = "channel.channel_points_custom_reward_redemption.add";
//         public const string Version = "1";
//     }
//
//     private static class EventSubRaid
//     {
//         public const string Id = "channel.raid";
//         public const string Version = "1";
//     }
//
//     private static class EventSubChannelCheer
//     {
//         public const string Id = "channel.cheer";
//         public const string Version = "1";
//     }
// }
