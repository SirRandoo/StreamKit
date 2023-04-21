using System;

namespace StreamKit.Platform.Twitch.Helix
{
    public class CheermotesData
    {
        /// <summary>
        ///     A string representing the text that must come before a given
        ///     number in order to use a given cheermote.
        /// </summary>
        /// <example>
        ///     In the example cheer of "Cheer100", "Cheer" would be the prefix
        ///     for Twitch's gem cheermote, and "100" would be the number being
        ///     cheered.
        /// </example>
        public string Prefix { get; set; }

        /// <summary>
        ///     A list of tier levels the cheermote supports. Each tier
        ///     represents the minimum amount of bits that must be cheered before
        ///     the accompanying assets will be displayed.
        /// </summary>
        public CheermoteTierData[] Tiers { get; set; }

        /// <summary>
        ///     The type of cheermote being represented.
        /// </summary>
        public CheermoteType Type { get; set; }

        /// <summary>
        ///     An integer representing the relative order a cheermote may be
        ///     displayed in the bits card -- the popup displayed when the user
        ///     clicks the bits icon near the chat box.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     The date and time the cheermote was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        ///     Whether the cheermote provides a charitable contribution when
        ///     used during a charity campaign.
        /// </summary>
        public bool IsCharitable { get; set; }
    }
}