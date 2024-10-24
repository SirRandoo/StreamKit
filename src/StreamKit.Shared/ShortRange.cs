namespace StreamKit.Shared;

/// <summary>
///     A specialized struct representing an upper and lower bound within a <see cref="short" />'s
///     minimum and maximum values.
/// </summary>
/// <param name="minimum">The lower bound of the range.</param>
/// <param name="maximum">The upper bound of the range.</param>
public class ShortRange(short minimum = short.MinValue, short maximum = short.MaxValue)
{
    /// <summary>
    ///     The lower bound of the range.
    /// </summary>
    public short Minimum { get; set; } = minimum;

    /// <summary>
    ///     The upper bound of the range.
    /// </summary>
    public short Maximum { get; set; } = maximum;
}
