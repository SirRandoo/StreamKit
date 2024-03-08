using System;
using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Shared.Core.Settings;

/// <summary>
///     <para>
///         A class for housing point decay settings.
///     </para>
///     <para>
///         Point decay settings are settings that affect how viewer's income will decay.
///     </para>
/// </summary>
public class PointDecaySettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the point decay settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The period of time that must pass before viewer's income will start decaying.
    /// </summary>
    public TimeSpan Period { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     A percentage indicating the amount of points viewers will lose when they aren't actively
    ///     participating in chat.
    /// </summary>
    public float DecayPercent { get; set; }

    /// <summary>
    ///     A fixed amount indicating the amount of points viewers will lose when they aren't actively
    ///     participating in chat.
    /// </summary>
    /// <remarks>
    ///     Fixed point decays are removed <i>after</i> the result of the percentage based decay
    ///     calculation.
    /// </remarks>
    public int FixedAmount { get; set; }

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
