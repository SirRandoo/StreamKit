namespace StreamKit.Platform.Twitch.Helix
{
    public class HelixPagination
    {
        /// <summary>
        ///     The cursor used to get the next page of results.
        /// </summary>
        /// <remarks>
        ///     The cursor should always be passed to endpoints that support
        ///     pagination in order to continue getting information in a given
        ///     set of results. The cursor is typically passed to the 'after'
        ///     query parameter, but may differ from endpoint to endpoint.
        /// </remarks>
        public string Cursor { get; set; }
    }
}
