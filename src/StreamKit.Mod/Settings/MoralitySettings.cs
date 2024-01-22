using StreamKit.Api;
using StreamKit.Api.Attributes;

namespace StreamKit.Mod.Settings;

/// <summary>
///     <para>
///         A class for housing morality settings.
///     </para>
///     <para>
///         Karma settings are settings that affect how "karma," the mod's short term limiter on bad
///         purchases. This limiter directly affects how much income viewers can accumulate, and
///         optionally, how much wealth viewers are stockpiling.
///     </para>
/// </summary>
public class MoralitySettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the karma settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The amount of karma viewers will start with when they either:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>first join chat</term>
    ///         </item>
    ///         <item>
    ///             <term>have their karma reset</term>
    ///         </item>
    ///     </list>
    /// </summary>
    [Label("Starting karma")]
    [Description("The amount of karma viewers will start off with when they first interact with the mod, or their data is reset.")]
    public short StartingKarma { get; set; } = 10;

    /// <summary>
    ///     A range representing the minimum and maximum amount of karma viewers can have.
    /// </summary>
    [Label("Karma range")]
    [Description("A range representing the minimum and maximum amount of karma viewers can have.")]
    public ShortRange KarmaRange { get; set; } = new(-100, 100);

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
