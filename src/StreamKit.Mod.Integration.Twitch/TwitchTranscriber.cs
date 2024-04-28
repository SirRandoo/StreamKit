using System;
using System.Collections.Generic;
using StreamKit.Common.Data.Abstractions;
// using TwitchLib.Client.Models;

namespace StreamKit.Mod.Integration.Twitch;

internal static class TwitchTranscriber
{
    // public static IMessage TranscribeMessage(ChatMessage message) => new TwitchMessage
    // {
    //     Id = message.Id,
    //     Author = new TwitchUser { Id = message.UserId, Name = message.Username, LastSeen = DateTime.UtcNow, Privileges = TranscribeBadges(message.BadgeInfo) },
    //     Content = message.Message,
    //     ReceivedAt = DateTime.UtcNow
    // };

    private static UserPrivileges TranscribeBadges(List<KeyValuePair<string, string>> badges)
    {
        var root = UserPrivileges.None;

        foreach (KeyValuePair<string, string> pair in badges)
        {
            string badge = pair.Key;

            switch (badge)
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
        public string Id { get; set; }

        /// <inheritdoc />
        public IUser Author { get; set; }

        /// <inheritdoc />
        public string Content { get; set; }

        /// <inheritdoc />
        public DateTime ReceivedAt { get; set; }
    }

    private sealed class TwitchUser : IUser
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public long Points { get; set; }

        /// <inheritdoc />
        public short Karma { get; set; }

        /// <inheritdoc />
        public string Platform { get; set; }

        /// <inheritdoc />
        public DateTime LastSeen { get; set; }

        /// <inheritdoc />
        public UserPrivileges Privileges { get; set; }

        /// <inheritdoc />
        public List<ITransaction> Transactions { get; set; }
    }
}
