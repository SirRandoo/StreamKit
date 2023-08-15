namespace StreamKit.Platform.Twitch.Helix;

public class CheermoteTierData
{
    /// <summary>
    ///     The minimum amount that must be cheered in order for this tier to
    ///     be displayed in chat.
    /// </summary>
    public int MinBits { get; set; }

    /// <summary>
    ///     The tier level being represented.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///     The image sets for the cheermote.
    /// </summary>
    public CheermoteImageData Images { get; set; }

    /// <summary>
    ///     The hex color of the color associated with this tier.
    /// </summary>
    public string Color { get; set; }

    /// <summary>
    ///     Whether a user can reach this level through cheers.
    /// </summary>
    public bool CanCheer { get; set; }

    /// <summary>
    ///     Whether this tier level is displayed in the bits card -- the
    ///     popup that appears when a user clicks the bits icon near the chat
    ///     box.
    /// </summary>
    public bool ShowInBitsCard { get; set; }
}
