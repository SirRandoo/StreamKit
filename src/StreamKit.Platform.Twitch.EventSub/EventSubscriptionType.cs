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

using System.Runtime.Serialization;

namespace StreamKit.Platform.Twitch.EventSub
{
    public enum EventSubscriptionType
    {
        /// <summary>
        ///     A broadcaster updates their channel properties e.g., category,
        ///     title, mature flag, broadcast, or language.
        /// </summary>
        [EnumMember(Value = "channel.update")]
        ChannelUpdate,

        /// <summary>
        ///     A specified channel receives a follow.
        /// </summary>
        [EnumMember(Value = "channel.follow")]
        ChannelFollow,

        /// <summary>
        ///     A notification when a specified channel receives a subscriber.
        ///     This does not include resubscribes.
        /// </summary>
        [EnumMember(Value = "channel.subscribe")]
        ChannelSubscribe,

        /// <summary>
        ///     A notification when a subscription to the specified channel ends.
        /// </summary>
        [EnumMember(Value = "channel.subscription.end")]
        ChannelSubscriptionEnd,

        /// <summary>
        ///     A notification when a viewer gifts a subscription to one or more
        ///     users in the specified channel.
        /// </summary>
        [EnumMember(Value = "channel.subscription.gift")]
        ChannelSubscriptionGift,

        /// <summary>
        ///     A notification when a user sends a re-subscription chat message
        ///     in a specific channel.
        /// </summary>
        [EnumMember(Value = "channel.subscription.message")]
        ChannelSubscriptionMessage,

        /// <summary>
        ///     A user cheers on the specified channel.
        /// </summary>
        [EnumMember(Value = "channel.cheer")]
        ChannelCheer,

        /// <summary>
        ///     A broadcaster raids another broadcaster's channel.
        /// </summary>
        [EnumMember(Value = "channel.raid")]
        ChannelRaid,

        /// <summary>
        ///     A viewer is banned from the specified channel.
        /// </summary>
        [EnumMember(Value = "channel.ban")]
        ChannelBan,

        /// <summary>
        ///     A viewer is unbanned from the specified channel.
        /// </summary>
        [EnumMember(Value = "channel.unban")]
        ChannelUnban,

        /// <summary>
        ///     Moderator privileges were added to a user on a specified channel.
        /// </summary>
        [EnumMember(Value = "channel.moderator.add")]
        ChannelModeratorAdd,

        /// <summary>
        ///     Moderator privileges were removed from a user on the specified
        ///     channel.
        /// </summary>
        [EnumMember(Value = "channel.moderator.remove")]
        ChannelModeratorRemove,

        /// <summary>
        ///     A custom channel points reward has been created for the specified
        ///     channel.
        /// </summary>
        [EnumMember(Value = "channel.channel_points_custom_reward.add")]
        ChannelPointsCustomRewardAdd,

        /// <summary>
        ///     A custom channel points reward has been updated for the specified
        ///     channel.
        /// </summary>
        [EnumMember(Value = "channel.channel_points_custom_reward.update")]
        ChannelPointsCustomRewardUpdate,

        /// <summary>
        ///     A custom channel points reward has been removed from the
        ///     specified channel.
        /// </summary>
        [EnumMember(Value = "channel.channel_points_custom_reward.remove")]
        ChannelPointsCustomRewardRemove,

        /// <summary>
        ///     A viewer has redeemed a custom channel points reward on the
        ///     specified channel.
        /// </summary>
        [EnumMember(Value = "channel.channel_points_custom_reward_redemption.add")]
        ChannelPointsCustomRewardRedemptionAdd,

        /// <summary>
        ///     A redemption of the channel points custom reward has been updated
        ///     for the specified channel.
        /// </summary>
        [EnumMember(Value = "channel.channel_points_custom_reward_redemption.remove")]
        ChannelPointsCustomRewardRedemptionUpdate,

        /// <summary>
        ///     A poll started on a specified channel.
        /// </summary>
        [EnumMember(Value = "channel.poll.begin")]
        ChannelPollBegin,

        /// <summary>
        ///     Users respond to a poll on a specified channel.
        /// </summary>
        [EnumMember(Value = "channel.poll.progress")]
        ChannelPollProgress,

        /// <summary>
        ///     A poll ended on a specified channel.
        /// </summary>
        [EnumMember(Value = "channel.poll.end")]
        ChannelPollEnd,

        /// <summary>
        ///     A prediction started on a specified channel.
        /// </summary>
        [EnumMember(Value = "channel.prediction.begin")]
        ChannelPredictionBegin,

        /// <summary>
        ///     Users participated in a prediction on a specified channel.
        /// </summary>
        [EnumMember(Value = "channel.prediction.progress")]
        ChannelPredictionProgress,

        /// <summary>
        ///     A prediction was locked on a specified channel.
        /// </summary>
        [EnumMember(Value = "channel.prediction.lock")]
        ChannelPredictionLock,

        /// <summary>
        ///     A prediction ended on a specified channel.
        /// </summary>
        [EnumMember(Value = "channel.prediction.end")]
        ChannelPredictionEnd,

        /// <summary>
        ///     Sends an event notification when a user donates to the
        ///     broadcaster's charity campaign.
        /// </summary>
        [EnumMember(Value = "channel.charity_campaign.donate")]
        CharityDonation,

        /// <summary>
        ///     Sends an event notification when the broadcaster starts a charity
        ///     campaign.
        /// </summary>
        [EnumMember(Value = "channel.charity_campaign.start")]
        CharityCampaignStart,

        /// <summary>
        ///     Sends an event notification when progress is made towards the
        ///     campaign's goal or when the broadcaster changes the fundraising
        ///     goal.
        /// </summary>
        [EnumMember(Value = "channel.charity_campaign.progress")]
        CharityCampaignProgress,

        /// <summary>
        ///     Sends an event notification when the broadcaster stops a charity
        ///     campaign.
        /// </summary>
        [EnumMember(Value = "channel.charity_campaign.stop")]
        CharityCampaignStop,

        /// <summary>
        ///     An entitlement for a drop is granted to a user.
        /// </summary>
        [EnumMember(Value = "drop.entitlement.grant")]
        DropEntitlementGrant,

        /// <summary>
        ///     A bits transaction occurred for a specified Twitch Extension.
        /// </summary>
        [EnumMember(Value = "extension.bits_transaction.create")]
        ExtensionBitsTransactionCreate,

        /// <summary>
        ///     Gets notified when a broadcaster beings a goal.
        /// </summary>
        [EnumMember(Value = "channel.goal.begin")]
        GoalBegin,

        /// <summary>
        ///     Gets notified when progress (either positive or negative) is made
        ///     towards a broadcaster's goal.
        /// </summary>
        [EnumMember(Value = "channel.goal.progress")]
        GoalProgress,

        /// <summary>
        ///     Gets notified when a broadcaster ends a goal.
        /// </summary>
        [EnumMember(Value = "channel.goal.end")]
        GoalEnd,

        /// <summary>
        ///     A Hype Train begins on the specified channel.
        /// </summary>
        [EnumMember(Value = "channel.hype_train.begin")]
        HypeTrainBegin,

        /// <summary>
        ///     A Hype Train makes progress on the specified channel.
        /// </summary>
        [EnumMember(Value = "channel.hype_train.progress")]
        HypeTrainProgress,

        /// <summary>
        ///     A Hype Train ends on the specified channel.
        /// </summary>
        [EnumMember(Value = "channel.hype_train.end")]
        HypeTrainEnd,

        /// <summary>
        ///     Sends a notification when the broadcaster actives Shield Mode.
        /// </summary>
        [EnumMember(Value = "channel.shield_mode.begin")]
        ShieldModeBegin,

        /// <summary>
        ///     Sends a notification when the broadcaster deactivates Shield
        ///     Mode.
        /// </summary>
        [EnumMember(Value = "channel.shield_mode.end")]
        ShieldModeEnd,

        /// <summary>
        ///     Sends a notification when the specified broadcaster sends a
        ///     Shoutout.
        /// </summary>
        [EnumMember(Value = "channel.shoutout.create")]
        ShoutoutCreate,

        /// <summary>
        ///     Sends a notification when the specified broadcaster receives a
        ///     Shoutout.
        /// </summary>
        [EnumMember(Value = "channel.shoutout.received")]
        ShoutoutReceived,

        /// <summary>
        ///     The specified broadcaster starts a stream.
        /// </summary>
        [EnumMember(Value = "stream.online")]
        StreamOnline,

        /// <summary>
        ///     The specified broadcaster stops a stream.
        /// </summary>
        [EnumMember(Value = "stream.offline")]
        StreamOffline,

        /// <summary>
        ///     A user's authorization has been granted to your client id.
        /// </summary>
        [EnumMember(Value = "user.authorization.grant")]
        UserAuthorizationGrant,

        /// <summary>
        ///     A user's authorization has been revoked for your client id.
        /// </summary>
        [EnumMember(Value = "user.authorization.revoke")]
        UserAuthorizationRevoke,

        /// <summary>
        ///     A user has updated their account.
        /// </summary>
        [EnumMember(Value = "user.update")]
        UserUpdate
    }
}
