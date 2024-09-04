using JetBrains.Annotations;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents the different states a custom reward's fulfillment status can be in.
/// </summary>
[PublicAPI]
public enum CustomRewardRedemptionStatus
{
    /// <summary>
    ///     The custom reward is currently unfulfilled.
    /// </summary>
    /// <remarks>
    ///     Developers who've created a custom reward that doesn't automatically fulfill itself should
    ///     ensure redemptions are fulfilled when the operation associated with the reward's redemption
    ///     completes.
    /// </remarks>
    Unfulfilled,

    /// <summary>
    ///     The custom reward has been considered "fulfilled."
    /// </summary>
    Fulfilled
}