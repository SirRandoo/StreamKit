using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Shared.Core.Settings;

// TODO: These settings should contain a "user type" setting.
// TODO: These settings should contain a "time watching" setting.
// TODO: These settings should contain a "multiplier" setting.
// TODO: These settings should contain a "flat value" setting.
// TODO: These settings should contain a "karma" requirement setting.
// TODO: These settings should contain a "reputation" requirement setting.

public class PointTierSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the point tier settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
