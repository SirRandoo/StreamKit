namespace StreamKit.Parsers.Irc;

/// <inheritdoc cref="IMessageTag"/>
public record MessageTag(string Name, string Value) : IMessageTag
{
    /// <inheritdoc cref="IMessageTag.Value"/>
    public virtual string Value { get; set; }
}
