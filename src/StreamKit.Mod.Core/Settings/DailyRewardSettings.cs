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
using StreamKit.Common.Data.Abstractions;
using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Core.Settings;

public enum DailyRewardType { [Label("Mystery box")] MysteryBox, [Label("Points")] FixedPoints }

public abstract class DailyRewardSettings : IComponentSettings
{
    /// <summary>
    ///     The type of reward that will be awarded to the viewer.
    /// </summary>
    [Experimental]
    public DailyRewardType Type { get; set; }

    /// <summary>
    ///     The likelihood a viewer will be considered for the award.
    /// </summary>
    /// <remarks>
    ///     Systems that use this property should consider this the "consideration likelihood," as opposed
    ///     to the likelihood they're receive the award, since rewards may have requirements viewers must
    ///     meet.
    /// </remarks>
    [Experimental]
    public float Chance { get; set; }

    /// <summary>
    ///     Whether the reward requires the viewer to have been active in a certain number of concurrent
    ///     streams in order to be considered for the reward.
    /// </summary>
    [Experimental]
    public bool HasStreamThreshold { get; set; }

    /// <summary>
    ///     The number of concurrent streams the viewer has to be active in in-order to be considered for
    ///     the reward.
    /// </summary>
    [Experimental]
    public int StreamThreshold { get; set; }

    /// <summary>
    ///     Whether viewers have to be active for a certain amount of time before being considered for the
    ///     reward.
    /// </summary>
    [Experimental]
    public bool HasTimeThreshold { get; set; }

    /// <summary>
    ///     The amount of time viewers have to be active for in order to be considered for the reward.
    /// </summary>
    [Experimental]
    public TimeSpan TimeThreshold { get; set; }

    /// <summary>
    ///     Whether viewers have to have a certain roles in order to be considered for the role.
    /// </summary>
    [Experimental]
    public bool HasRequiredRoles { get; set; }

    /// <summary>
    ///     Whether viewers can have any of the roles specified in order to be considered for the reward.
    /// </summary>
    [Experimental]
    public bool RequireAnyRoles { get; set; }

    /// <summary>
    ///     The roles the user must have in order to be considered for the reward. If
    ///     <see cref="RequireAnyRoles" /> is <c>true</c>, viewers must have at least one of the listed
    ///     roles in order to be considered for the reward, as opposed to requiring all the listed roles.
    /// </summary>
    [Experimental]
    public UserRoles RequiredRoles { get; set; }

    /// <inheritdoc />
    public abstract int Version { get; set; }
}

public class MysteryBoxDailyRewardSetting : DailyRewardSettings
{
    public const int LatestVersion = 1;

    /// <summary>
    ///     The minimum and maximum number of points viewers may receive from the mystery box.
    /// </summary>
    [Experimental]
    public IntRange Range { get; set; }

    /// <inheritdoc />
    public override int Version { get; set; } = LatestVersion;
}

public class FixedPointsDailyRewardSetting : DailyRewardSettings
{
    public const int LatestVersion = 1;

    /// <summary>
    ///     The amount of points the viewer will receive should they meet the reward's requirements.
    /// </summary>
    [Experimental]
    public int Points { get; set; }

    /// <inheritdoc />
    public override int Version { get; set; } = LatestVersion;
}
