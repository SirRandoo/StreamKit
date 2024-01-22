// MIT License
//
// Copyright (c) 2023 SirRandoo
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

using StreamKit.Api;
using StreamKit.Api.Attributes;

namespace StreamKit.Mod.Settings;

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
    [Label("Enable pawn pool")]
    [Description("Whether pawns that joined the colony are entered into a 'pool'.")]
    [Description("Viewers can subsequently claim a pawn from the pool, and optionally customize it through purchases.")]
    public bool Pooling { get; set; } = true;

    /// <summary>
    ///     Whether pawns claimed by viewers vacation from the colony when their viewer isn't active in
    ///     chat.
    /// </summary>
    [Label("Enable pawn vacationing")]
    [Description("Whether pawns will 'take a vacation' from the colony when their viewer isn't active in chat.")]
    public bool Vacationing { get; set; } = true;

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
