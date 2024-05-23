using System;
using System.Collections.Generic;
using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Shared.Core.Settings;

/// <summary>
///     <para>
///         A class for housing point settings.
///     </para>
///     <para>
///         Point settings are settings that affect how the mod distributes wealth to viewers, either
///         by restricting the amount of wealth given to a certain viewer, or by restricting wealth
///         distribution altogether.
///     </para>
/// </summary>
public class PointSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the point settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The amount of points viewers will receive they either:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>first join chat</term>
    ///         </item>
    ///         <item>
    ///             <term>have their balance reset</term>
    ///         </item>
    ///     </list>
    /// </summary>
    [Label("Starting balance")]
    [Description("The amount of points viewers will start with when they first interact with the mod, or their data is otherwise reset.")]
    public int StartingBalance { get; set; } = 100;

    /// <summary>
    ///     Whether the mod is currently distributing wealth to viewers.
    /// </summary>
    [Label("Distribute points periodically")]
    [Description("Whether the mod is distributing points on a regular, user defined, interval.")]
    public bool IsDistributing { get; set; } = true;

    /// <summary>
    ///     Whether the viewers will always enough points for whatever they want to purchase.
    /// </summary>
    [Label("Infinite points")]
    [Description("When enabled, viewers will always have enough points for whatever they choose to purchase.")]
    [Description("You probably don't want to enable this setting.")]
    public bool InfinitePoints { get; set; }

    /// <summary>
    ///     The amount of time that has to pass before viewers are awarded wealth.
    /// </summary>
    [Label("Point distribution interval")]
    [Description("The amount of time that must pass after points were distributed before the mod will more points.")]
    public TimeSpan RewardInterval { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     The amount of points viewers will receive when wealth is distributed.
    /// </summary>
    [Label("Distributed amount")]
    [Description("The amount of points that are given to each viewer when points are being distributed.")]
    public int RewardAmount { get; set; } = 100;

    /// <summary>
    ///     Whether viewers must participate in chat in order to receive wealth.
    /// </summary>
    [Label("Requires active participation")]
    [Description("Whether only active viewers will receive points when the mod distributes points.")]
    public bool ParticipationRequired { get; set; } = true;

    /// <summary>
    ///     The various tiers of point decay that can occur.
    /// </summary>
    [Label("Point decay")]
    [Description("A collection of settings that affect how viewer's earnings decay.")]
    [Description(
        "These settings can, optionally, decay a viewer's balance, but their main purpose is the modify how much points viewers earn depending on certain factors."
    )]
    public List<PointDecaySettings> PointDecaySettings { get; set; } = [];

    [Label("Point tiers")]
    [Description("A collection of settings that affect the 'point tiers' of the point system.")]
    [Description("Point tiers, more or less, modify how many points certain viewers can get depending on what type of viewer they are, like if they're a moderator.")]
    public List<PointTierSettings> PointTierSettings { get; set; } = [];

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
