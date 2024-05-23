using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Shared.Core.Settings;

public class PawnSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the pawn settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     Whether the mod's "pawn poll" system is enabled. The pawn pooling system is a system that
    ///     allows viewers to claim a pawn once it enters the queue, optionally with some streamer
    ///     intervention.
    /// </summary>
    [Label("Enable pawn pool")]
    [Description("Whether pawns that joined the colony are entered into a 'pool'.")]
    [Description("Viewers can subsequently claim a pawn from the pool, and optionally customize it through purchases.")]
    public bool Pooling { get; set; } = true;

    /// <summary>
    ///     Whether pawns claimed by viewers vacation from the colony when their viewer isn't active in
    ///     chat.
    /// </summary>
    [Experimental]
    [Label("Enable pawn vacationing")]
    [Description("Whether pawns will 'take a vacation' from the colony when their viewer isn't active in chat.")]
    public bool Vacationing { get; set; } = true;

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
