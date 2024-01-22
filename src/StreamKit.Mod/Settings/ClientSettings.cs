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
using UnityEngine;

namespace StreamKit.Mod.Settings;

/// <summary>
///     <para>
///         A class housing client-side settings.
///     </para>
///     <para>
///         Client-side settings are settings that can affect only the physical appearance of the mod's
///         displays, change the output of non-essential methods, like whether viewer's name colors are
///         stored and used.
///     </para>
/// </summary>
public class ClientSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the client settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    [Label("Gizmo puff")]
    [Description("When enabled, the interactive gizmo will create a small puff of smoke whenever it materializes an item.")]
    public bool GizmoPuff { get; set; } = true;

    [InternalSetting] public Vector2 PollPosition { get; set; } = new(-1, -1);

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
