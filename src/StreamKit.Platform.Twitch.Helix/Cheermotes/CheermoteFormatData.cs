namespace StreamKit.Platform.Twitch.Helix
{
    public class CheermoteFormatData
    {
        /// <summary>
        ///     The various sizes the animated cheermote can be displayed at.
        /// </summary>
        public CheermoteSizeData Animated { get; set; }

        /// <summary>
        ///     The various sizes the cheermote can be displayed at.
        /// </summary>
        public CheermoteSizeData Static { get; set; }
    }
}