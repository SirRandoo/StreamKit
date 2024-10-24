namespace StreamKit.Shared;

/// <summary>
///     A specialized struct representing an upper and lower bound within a <see cref="byte" />'s
///     minimum and maximum values.
/// </summary>
/// <param name="minimum">The lower bound of the range.</param>
/// <param name="maximum">The upper bound of the range.</param>
public struct ByteRange(byte minimum = byte.MinValue, byte maximum = byte.MaxValue)
{
    /// <summary>
    ///     The lower bound of the range.
    /// </summary>
    public byte Minimum = minimum;

    /// <summary>
    ///     The upper bound of the range.
    /// </summary>
    public byte Maximum = maximum;
}