namespace StreamKit.Parsers.Irc
{
    /// <inheritdoc cref="IMessageTag"/>
    public class MessageTag : IMessageTag
    {
        /// <inheritdoc cref="IMessageTag.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="IMessageTag.Value"/>
        public virtual string Value { get; set; }
    }
}
