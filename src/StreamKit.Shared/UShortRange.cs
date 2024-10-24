namespace StreamKit.Shared;

/// <summary>
///     A specialized struct representing an upper and lower bound within a <see cref="ushort" />'s
///     minimum and maximum values.
/// </summary>
/// <param name="minimum">The lower bound of the range.</param>
/// <param name="maximum">The upper bound of the range.</param>
public struct UShortRange(ushort minimum = ushort.MinValue, ushort maximum = ushort.MaxValue)
{
    /// <summary>
    ///     The lower bound of the range.
    /// </summary>
    public ushort Minimum = minimum;

    /// <summary>
    ///     The upper bound of the range.
    /// </summary>
    public ushort Maximum = maximum;
}