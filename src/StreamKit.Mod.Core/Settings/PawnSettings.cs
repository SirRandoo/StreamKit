// MIT License
//
// Copyright (c) 2024 SirRandoo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using StreamKit.Common.Data.Abstractions;
using StreamKit.Mod.Api;

namespace StreamKit.Mod.Core.Settings;

public class PawnSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the pawn settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     Whether the mod's "pawn poll" system is enabled. The pawn pooling system is a system that
    ///     allows viewers to claim a pawn once it enters the queue, optionally with some streamer
    ///     intervention.
    /// </summary>
    public bool Pooling { get; set; } = true;

    /// <summary>
    ///     The user types who are allowed to claim a pawn from the pawn pool.
    /// </summary>
    public UserRoles PoolingRestrictions { get; set; } = UserRoles.None;

    /// <summary>
    ///     Whether pawns claimed by viewers vacation from the colony when their viewer isn't active in
    ///     chat.
    /// </summary>
    /// <remarks>
    ///     Pawns who are eligible for a vacation will not go on their vacation until if they're injured,
    ///     on a mental break, or otherwise incapacitated. Once they are fully healed, and capable of
    ///     walking, they'll go on their vacation if there's vacation slots available.
    /// </remarks>
    public bool Vacationing { get; set; } = true;

    /// <summary>
    ///     The total number of "reserved" pawns that will remain in the colony, even if they're eligible
    ///     for a vacation.
    /// </summary>
    /// <remarks>
    ///     Due to restrictions in RimWorld, this value must <b>always</b> be at least 1. If this value is
    ///     lower than 1, the game will consider the colony over. Players can still play on an empty
    ///     colony, but it's still jarring for the player.
    /// </remarks>
    /// <inheritdoc cref="Vacationing" path="/remarks" />
    public short TotalOverworkedPawns { get; set; } = 1;

    /// <summary>
    ///     Whether vacationing pawns will be called back to work during a crisis, like a raid.
    /// </summary>
    /// <inheritdoc cref="Vacationing" path="/remarks" />
    public bool EmergencyWorkCrisis { get; set; } = true;

    /// <inheritdoc />
    public int Version { get; set; } = LatestVersion;
}
