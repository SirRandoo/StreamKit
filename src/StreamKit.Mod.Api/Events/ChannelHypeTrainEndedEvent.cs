using System;
using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents progression in a hype train on the channel.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/params[@name='Platform' or 'User']" />
/// <param name="HypeTrainId">The id of the hype train on the platform.</param>
/// <param name="Level">The current level of the hype train.</param>
/// <param name="Total">The total number of points that have been contributed to the hype train.</param>
/// <param name="TopContributors">The users who've contributed the most points to the hype train.</param>
[PublicAPI]
public sealed record ChannelHypeTrainEndedEvent(
    IPlatform Platform,
    IUser User,
    string HypeTrainId,
    int Level,
    int Total,
    HypeTrainContributor[] TopContributors,
    DateTime StartedAt,
    DateTime EndedAt
) : PlatformEvent(Platform, User);
