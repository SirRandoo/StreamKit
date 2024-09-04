using JetBrains.Annotations;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a custom reward being updating within the channel.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/param[@name='Platform' or 'User']" />
/// <param name="RewardId">The id of the custom reward that was updated within the channel.</param>
/// <param name="IsEnabled">Whether the reward is enabled within the channel.</param>
/// <param name="IsPaused">Whether redemptions for the custom reward are paused.</param>
/// <param name="Cost">
///     The amount of channel points users will spend in order to "redeem" the custom
///     reward.
/// </param>
[PublicAPI]
public sealed record ChannelCustomRewardUpdatedEvent(IPlatform Platform, IUser User, string RewardId, bool IsEnabled, bool IsPaused, int Cost)
    : PlatformEvent(Platform, User);