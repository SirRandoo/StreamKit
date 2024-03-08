using System;
using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Shared.Core.Settings;

/// <summary>
///     <para>
///         A class for housing poll settings.
///     </para>
///     <para>
///         Poll settings are settings that affect how the mod tallies votes.
///     </para>
/// </summary>
public class PollSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the voting settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The total amount of time a poll will be active for.
    /// </summary>
    [Label("Duration")]
    [Description("The total amount of time a given poll will be active for.")]
    public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    ///     The maximum amount of options that can appear in a poll at any given time.
    /// </summary>
    [Label("Maximum options")]
    [Description("The maximum number of options that can be present in any given poll.")]
    public int MaximumOptions { get; set; } = 4;

    /// <summary>
    ///     Whether mod will attempt to use the platform's native polling instead of using a chat-based
    ///     poll.
    /// </summary>
    [Label("Prefer native polls")]
    [Description("If enabled, the mod will prefer creating polls on supported platforms instead of using a display in-game.")]
    public bool PreferNativePolls { get; set; }

    /// <summary>
    ///     Whether the mod should also show a window indicating the current poll being held.
    /// </summary>
    [Label("Show poll dialog")]
    [Description("When enabled, the mod will display a window of the active poll. This setting is ignored if 'prefer native polls' is enabled.")]
    public bool PollDialog { get; set; } = true;

    /// <summary>
    ///     Whether the poll dialog will use a larger font size when displaying options.
    /// </summary>
    [Label("Use large text in poll window")]
    [Description("When enabled, a larger text will be used in the poll dialog. This setting can be useful for streamers.")]
    public bool LargeText { get; set; }

    /// <summary>
    ///     Whether the poll dialog will animate votes as they come in.
    /// </summary>
    [Label("Animate votes")]
    [Description("When enabled, the poll dialog will animate the amount of votes each choice has.")]
    [Description(
        "This means that when a viewer votes for an option, the dialog will animate a bar overtop of the choice gradually increasing relative to all other votes."
    )]
    public bool AnimateVotes { get; set; } = true;

    /// <summary>
    ///     Whether the mod will randomly generate polls from the game's event list, including mod events.
    /// </summary>
    [Label("Generate random polls")]
    [Description("When enabled, the mod will generate random polls from the game's internal event list, including events added by mods.")]
    public bool RandomPolls { get; set; } = true;

    /// <summary>
    ///     Whether mod will post active polls in chat.
    /// </summary>
    [Label("Options in chat")]
    [Description("When enabled, the mod will send poll options in chat alongside the number viewers have to put in chat to vote for said option.")]
    [Description("This setting is ignored if 'prefer native polls' is enabled.")]
    public bool OptionsInChat { get; set; } = true;

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
