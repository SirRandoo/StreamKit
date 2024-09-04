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

/// <summary>
///     <para>
///         A class for housing point decay settings.
///     </para>
///     <para>
///         Point decay settings are settings that affect how viewer's income will decay.
///     </para>
/// </summary>
public class PointDecaySettings : IIdentifiable, IComponentSettings
{
    /// <summary>
    ///     The latest version of the point decay settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The period of time that must pass before viewer's income will start decaying.
    /// </summary>
    [Experimental]
    public TimeSpan Period { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     A percentage indicating the amount of points viewers will lose when they aren't actively
    ///     participating in chat.
    /// </summary>
    [Experimental]
    public float DecayPercent { get; set; }

    /// <summary>
    ///     A fixed amount indicating the amount of points viewers will lose when they aren't actively
    ///     participating in chat.
    /// </summary>
    /// <remarks>
    ///     Fixed point decays are removed <i>after</i> the result of the percentage based decay
    ///     calculation.
    /// </remarks>
    [Experimental]
    public int FixedAmount { get; set; }

    /// <inheritdoc />
    public int Version { get; set; } = LatestVersion;

    /// <inheritdoc />
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <inheritdoc />
    public string Name { get; set; } = "My new setting";
}
