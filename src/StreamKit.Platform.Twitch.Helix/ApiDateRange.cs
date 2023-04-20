using System;

namespace StreamKit.Platform.Twitch.Helix
{
    public class ApiDateRange
    {
        /// <summary>
        ///     The date and time the associated information was first created
        ///     within Twitch's servers.
        /// </summary>
        public DateTime StartedAt { get; set; }

        /// <summary>
        ///     The date and time the associated information was last updated
        ///     within Twitch's servers.
        /// </summary>
        public DateTime EndedAt { get; set; }
    }
}
