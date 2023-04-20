using System;

namespace StreamKit.Platform.Twitch.Helix.Analytics
{
    public class ExtensionAnalyticsData
    {
        /// <summary>
        ///     The id that identifies the extension that the report was
        ///     generated for.
        /// </summary>
        public string ExtensionId { get; set; }

        /// <summary>
        ///     A unique url that can be used to download the report.
        /// </summary>
        /// <remarks>
        ///     The url provided is temporary, and will expire after a certain
        ///     amount of time has elapsed. For the exact window you may use the
        ///     url you can refer to Twitch's official documentation at
        ///     https://dev.twitch.tv/docs/api/reference/#get-extension-analytics
        /// </remarks>
        public Uri Url { get; set; }

        /// <summary>
        ///     The type of report being presented.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     The reporting window's start and end dates.
        /// </summary>
        public ApiDateRange DateRange { get; set; }
    }
}
