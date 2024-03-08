using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Shared.Core.Settings;

public class CommandSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the command settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    [Label("Command prefix")]
    [Description("One or more characters that must be present at the start of a message in chat in order for the mod to recognize it as a command.")]
    public string Prefix { get; set; } = "!";

    [Label("Use emojis in command responses")]
    [Description("When enabled, the mod will use emojis in place of some text that can be represented as one or more emojis. Disabling this may")]
    [Description("cause some information to be omitted from some command responses.")]
    public bool UseEmojis { get; set; } = true;

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
