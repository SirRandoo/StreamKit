using System;
using JetBrains.Annotations;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a custom reward being redeemed within the channel.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/params[@name='Platform' or 'User']" />
/// <param name="RewardId">The id of the custom reward that was redeemed by the user.</param>
/// <param name="RedemptionStatus">The current redemption status for the user's redeem.</param>
/// <param name="RedeemedAt">The moment in time the user redeemed the custom reward.</param>
/// <param name="UserInput">
///     The optional text the user could've supplied while redeeming the custom
///     reward. If the user didn't supply a message, the value of this property will be <c>null</c>.
/// </param>
[PublicAPI]
public sealed record ChannelCustomRewardRedemptionEvent(
    IPlatform Platform,
    IUser User,
    string RewardId,
    CustomRewardRedemptionStatus RedemptionStatus,
    DateTimeOffset RedeemedAt,
    string? UserInput = null
) : PlatformEvent(Platform, User);