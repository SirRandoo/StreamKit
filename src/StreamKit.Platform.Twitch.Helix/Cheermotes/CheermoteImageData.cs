namespace StreamKit.Platform.Twitch.Helix;

public class CheermoteImageData
{
    /// <summary>
    ///     The cheermotes that will be displayed to users with a dark theme
    ///     enabled.
    /// </summary>
    public CheermoteFormatData Dark { get; set; }

    /// <summary>
    ///     The cheermotes that will be displayed to users with a light theme
    ///     enabled.
    /// </summary>
    public CheermoteFormatData Light { get; set; }
}
