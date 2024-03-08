using StreamKit.Common.Data.Abstractions;
using Verse;

namespace StreamKit.Mod.Shared.Extensions;

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
    ///     Returns a random value between the upper and lower bounds of the range.
    /// </summary>
    /// <param name="range">
    ///     A <see cref="ShortRange" /> instance indicating the upper and lower bounds a value can be in.
    /// </param>
    public static short GetRandomValue(this ShortRange range) => (short)Rand.Range(range.Minimum, range.Maximum);
}
