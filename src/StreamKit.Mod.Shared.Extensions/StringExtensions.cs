using Verse;

namespace StreamKit.Mod.Shared.Extensions;

/// <summary>
///     A collection of extension methods for strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     An alternative to <see cref="Gen.ToStringSafe{T}" /> that returns <see cref="string.Empty" />
    ///     instead of "null".
    /// </summary>
    /// <param name="instance">An instance of an object that's being turned into a string.</param>
    /// <typeparam name="T">The type of the object being turned into a string.</typeparam>
    /// <returns>
    ///     <see cref="string.Empty" /> if the object was <see langword="null" />, or the object's string
    ///     representation.
    /// </returns>
    public static string ToStringNullable<T>(this T? instance) => instance == null ? string.Empty : instance.ToString();
}
