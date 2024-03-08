using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;
using UnityEngine;

namespace StreamKit.Mod.Shared.Core.Settings;

/// <summary>
///     <para>
///         A class housing client-side settings.
///     </para>
///     <para>
///         Client-side settings are settings that can affect only the physical appearance of the mod's
///         displays, change the output of non-essential methods, like whether viewer's name colors are
///         stored and used.
///     </para>
/// </summary>
public class ClientSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the client settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    [Label("Gizmo puff")]
    [Description("When enabled, the interactive gizmo will create a small puff of smoke whenever it materializes an item.")]
    public bool GizmoPuff { get; set; } = true;

    [InternalSetting] public Vector2 PollPosition { get; set; } = new(-1, -1);

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
