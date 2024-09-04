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

/// <summary>
///     <para>
///         A class for housing morality settings.
///     </para>
///     <para>
///         Karma settings are settings that affect how "karma," the mod's short term limiter on bad
///         purchases. This limiter directly affects how much income viewers can accumulate, and
///         optionally, how much wealth viewers are stockpiling.
///     </para>
/// </summary>
public class MoralitySettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the karma settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     Whether the mod's karma system is enabled.
    /// </summary>
    /// <remarks>
    ///     The karma system is a system to provide immediate consequences for viewers who purchase harmful
    ///     events. Viewers will typically see a loss in income, and will have to purchase positive events
    ///     in order to repay their karmic debt.
    /// </remarks>
    public bool IsKarmaEnabled { get; set; }

    /// <summary>
    ///     The amount of karma viewers will start with when they either:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>first join chat</term>
    ///         </item>
    ///         <item>
    ///             <term>have their karma reset</term>
    ///         </item>
    ///     </list>
    /// </summary>
    public short StartingKarma { get; set; } = 10;

    /// <summary>
    ///     A range representing the minimum and maximum amount of karma viewers can have.
    /// </summary>
    public ShortRange KarmaRange { get; set; } = new(-100, 100);

    /// <summary>
    ///     Whether the mod's reputation system is enabled.
    /// </summary>
    /// <remarks>
    ///     The reputation system provides long-term consequences for viewers who have purchase negative
    ///     events in the past.
    /// </remarks>
    public bool IsReputationEnabled { get; set; } = true;

    /// <inheritdoc />
    public int Version { get; set; } = LatestVersion;
}
