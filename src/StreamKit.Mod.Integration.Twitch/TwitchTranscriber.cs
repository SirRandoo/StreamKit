using System;
using System.Collections.Generic;
using StreamKit.Common.Data.Abstractions;
using TwitchLib.EventSub.Core.Models.Chat;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;

namespace StreamKit.Mod.Integration.Twitch;

// TODO: The transcriber currently creates a new user for every chat message that gets transcribed.
//       The transcriber should instead use a system within StreamKit to fetch a user for the Twitch
//       platform, or create one if they don't exist, then return an updated version of said user.
//       Alternatively, the transcriber can generate a new user for every message, then replace the
//       user within said StreamKit system?

// TODO: The transcriber contains specialized classes for representing Twitch content.
//       These could potentially be extracted into the common classes within StreamKit,
//       since there's little to no reason for dataclasses to contain platform specific
//       information in a system that can't properly access it without breaking the
//       system's abstractions.

internal static class TwitchTranscriber
{
    private const string TwitchPlatformId = "streamkit.platform.twitch";

    public static IMessage TranscribeMessage(ChannelChatMessage message) => new TwitchMessage
    {
        Id = message.MessageId,
        Author = new TwitchUser
        {
            Id = message.ChatterUserId,
            Name = message.ChatterUserName,
            LastSeen = DateTime.UtcNow,
            Privileges = TranscribeBadges(message.Badges),
            Platform = TwitchPlatformId
        },
        Content = message.Message.Text,
        ReceivedAt = DateTime.UtcNow
    };

    private static UserPrivileges TranscribeBadges(params ChatBadge[] badges)
    {
        var root = UserPrivileges.None;

        foreach (ChatBadge badge in badges)
        {
            switch (badge.Id)
            {
                case "moderator":
                    root |= UserPrivileges.Moderator;

                    break;
                case "vip":
                    root |= UserPrivileges.Vip;

                    break;
                case "subscriber":
                    root |= UserPrivileges.Subscriber;

                    break;
                case "broadcaster":
                    root |= UserPrivileges.Moderator | UserPrivileges.Subscriber | UserPrivileges.Vip;

                    return root;
            }
        }

        return root;
    }

    private sealed class TwitchMessage : IMessage
    {
        /// <inheritdoc />
        public string Id { get; set; } = null!;

        /// <inheritdoc />
        public IUser Author { get; set; } = null!;

        /// <inheritdoc />
        public string Content { get; set; } = null!;

        /// <inheritdoc />
        public DateTime ReceivedAt { get; set; }
    }

    private sealed class TwitchUser : IUser
    {
        /// <inheritdoc />
        public string Id { get; set; } = null!;

        /// <inheritdoc />
        public string Name { get; set; } = null!;

        /// <inheritdoc />
        public long Points { get; set; }

        /// <inheritdoc />
        public short Karma { get; set; }

        /// <inheritdoc />
        public string Platform { get; set; } = null!;

        /// <inheritdoc />
        public DateTime LastSeen { get; set; }

        /// <inheritdoc />
        public UserPrivileges Privileges { get; set; }

        /// <inheritdoc />
        public List<ITransaction> Transactions { get; set; } = null!;
    }
}
