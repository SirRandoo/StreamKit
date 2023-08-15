using System;

namespace StreamKit.Parsers.Irc;

internal ref struct Boundaries
{
    public ReadOnlySpan<char> Tags { get; set; }
    public ReadOnlySpan<char> Prefix { get; set; }
    public ReadOnlySpan<char> Command { get; set; }
    public ReadOnlySpan<char> Params { get; set; }
}
