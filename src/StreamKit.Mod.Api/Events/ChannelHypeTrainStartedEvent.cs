using System;
using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a hype train starting on the channel.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/params[@name='Platform' or 'User']" />
/// <param name="HypeTrainId">The id of the hype train on the platform.</param>
/// <param name="Level">The current level of the hype train.</param>
/// <param name="Goal">
///     The current number of points required to advance to the next level in the hype
///     train.
/// </param>
/// <param name="Progress">The current number of points the hype train has.</param>
/// <param name="Total">The total number of points that have been contributed to the hype train.</param>
/// <param name="TopContributors">The users who've contributed the most points to the hype train.</param>
/// <param name="LastContributor">The user who last contributed to the hype train.</param>
[PublicAPI]
public sealed record ChannelHypeTrainStartedEvent(
    IPlatform Platform,
    IUser User,
    string HypeTrainId,
    int Level,
    int Goal,
    int Progress,
    int Total,
    HypeTrainContributor[] TopContributors,
    HypeTrainContributor LastContributor,
    DateTime StartedAt,
    DateTime ExpiresAt
) : PlatformEvent(Platform, User);
