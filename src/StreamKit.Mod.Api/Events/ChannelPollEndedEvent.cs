using System;
using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a poll within the channel ending.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/param[@name='Platform' or 'User']" />
/// <param name="PollId">The id of the poll that ended on the platform.</param>
/// <param name="Choices">
///     The choices that were in the poll, as well as the number of votes each choice
///     received.
/// </param>
/// <param name="StartedAt">The moment in time the poll started.</param>
[PublicAPI]
public sealed record ChannelPollEndedEvent(IPlatform Platform, IUser User, string PollId, ChannelPollChoice[] Choices, DateTimeOffset StartedAt)
    : PlatformEvent(Platform, User)
{
    /// <summary>
    ///     Returns the total number of votes cast on the poll.
    /// </summary>
    public int TotalVotes
    {
        get
        {
            var totalVotes = 0;

            for (var i = 0; i < Choices.Length; i++)
            {
                totalVotes += Choices[i].Votes;
            }

            return totalVotes;
        }
    }
}
