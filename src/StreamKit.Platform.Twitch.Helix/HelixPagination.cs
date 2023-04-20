namespace StreamKit.Platform.Twitch.Helix
{
    public class HelixPagination : IPaginationCursor
    {
        /// <inheritdoc cref="IPaginationCursor.Cursor"/>
        public string Cursor { get; set; }
    }
}
