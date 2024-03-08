using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Common.Data.Extensions;

/// <summary>
///     A collection of extension methods for modifying <see cref="ShortRange" />s without an
///     allocation.
/// </summary>
/// <remarks>
///     These extension methods mutate an existing <see cref="ShortRange" />, and thus aren't suitable
///     for operations that require immutable modifications.
/// </remarks>
public static class ShortRangeExtensions
{
    /// <summary>
    ///     Updates the minimum value for a given <see cref="ShortRange" />.
    /// </summary>
    /// <param name="range">The range being updated.</param>
    /// <param name="minimum">The new minimum value of the range.</param>
    /// <returns>The new, mutated, range.</returns>
    public static ref ShortRange SetMinimum(this ref ShortRange range, short minimum)
    {
        range.Minimum = minimum;

        return ref range;
    }

    /// <summary>
    ///     Updates the maximum value for a given <see cref="ShortRange" />.
    /// </summary>
    /// <param name="range">The range being updated.</param>
    /// <param name="maximum">The new maximum value of the range.</param>
    /// <returns>The new, mutated, range.</returns>
    public static ref ShortRange SetMaximum(this ref ShortRange range, short maximum)
    {
        range.Maximum = maximum;

        return ref range;
    }
}
