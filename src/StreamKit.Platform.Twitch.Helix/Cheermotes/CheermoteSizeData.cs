using System;

namespace StreamKit.Platform.Twitch.Helix;

public class CheermoteSizeData
{
    /// <summary>
    ///     The smallest size a cheermote can be displayed at (28x28 pixels).
    /// </summary>
    public Uri One { get; set; }

    /// <summary>
    ///     The second smallest size a cheermote can be displayed at (42x42
    ///     pixels).
    /// </summary>
    public Uri OnePointFive { get; set; }

    /// <summary>
    ///     The third size a cheermote can be displayed at (56x56 pixels).
    /// </summary>
    public Uri Two { get; set; }

    /// <summary>
    ///     The fourth size a cheermote can be displayed at (84x84 pixels).
    /// </summary>
    public Uri Three { get; set; }

    /// <summary>
    ///     The fifth size a cheermote can be displayed at (112x112 pixels).
    /// </summary>
    public Uri Four { get; set; }
}
