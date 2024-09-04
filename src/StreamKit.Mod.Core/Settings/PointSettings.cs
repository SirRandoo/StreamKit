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

using System;
using System.Collections.Generic;
using StreamKit.Mod.Api;

namespace StreamKit.Mod.Core.Settings;

/// <summary>
///     <para>
///         A class for housing point settings.
///     </para>
///     <para>
///         Point settings are settings that affect how the mod distributes wealth to viewers, either
///         by restricting the amount of wealth given to a certain viewer, or by restricting wealth
///         distribution altogether.
///     </para>
/// </summary>
public class PointSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the point settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     Whether the viewers will always enough points for whatever they want to purchase.
    /// </summary>
    public bool InfinitePoints { get; set; }

    /// <summary>
    ///     Whether the mod is currently distributing wealth to viewers.
    /// </summary>
    public bool IsDistributing { get; set; } = true;

    /// <summary>
    ///     The amount of points viewers will receive they either:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>first join chat</term>
    ///         </item>
    ///         <item>
    ///             <term>have their balance reset</term>
    ///         </item>
    ///     </list>
    /// </summary>
    public int StartingBalance { get; set; } = 100;

    /// <summary>
    ///     The amount of time that has to pass before viewers are awarded wealth.
    /// </summary>
    public TimeSpan RewardInterval { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     The amount of points viewers will receive when wealth is distributed.
    /// </summary>
    public int RewardAmount { get; set; } = 100;

    /// <summary>
    ///     Whether viewers must participate in chat in order to receive wealth.
    /// </summary>
    public bool ParticipationRequired { get; set; } = true;

    /// <summary>
    ///     Whether the mod's point decay is enabled.
    /// </summary>
    /// <remarks>
    ///     The point decay system gradually decreases an inactive viewer's balance after they've met
    ///     certain thresholds.
    /// </remarks>
    public bool HasPointDecay { get; set; }

    /// <summary>
    ///     The various tiers of point decay that can occur.
    /// </summary>
    public List<PointDecaySettings> PointDecaySettings { get; set; } = [];

    /// <summary>
    ///     Whether the mod should distribute points based on configurable tiers.
    /// </summary>
    public bool HasPointTiers { get; set; }

    /// <summary>
    ///     The various point tiers that exist.
    /// </summary>
    public List<PointTierSettings> PointTierSettings { get; set; } = [];

    /// <summary>
    ///     Whether the mod should hand out rewards.
    /// </summary>
    /// <remarks>
    ///     The mod's rewards system gives viewers certain rewards depending on the streamer's configured
    ///     settings.
    /// </remarks>
    public bool HasRewards { get; set; }

    /// <summary>
    ///     The various rewards that exist.
    /// </summary>
    public List<DailyRewardSettings> RewardSettings { get; set; } = [];

    /// <inheritdoc />
    public int Version { get; set; } = LatestVersion;
}
