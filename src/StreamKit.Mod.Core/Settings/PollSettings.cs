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
using System.Text.Json.Serialization;
using StreamKit.Mod.Api;

namespace StreamKit.Mod.Core.Settings;

/// <summary>
///     <para>
///         A class for housing poll settings.
///     </para>
///     <para>
///         Poll settings are settings that affect how the mod tallies votes.
///     </para>
/// </summary>
public class PollSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the voting settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The total amount of time a poll will be active for.
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    ///     The maximum amount of options that can appear in a poll at any given time.
    /// </summary>
    public int MaximumOptions { get; set; } = 4;

    /// <summary>
    ///     Whether mod will attempt to use the platform's native polling instead of using a chat-based
    ///     poll.
    /// </summary>
    public bool PreferNativePolls { get; set; }

    /// <summary>
    ///     Whether the mod should also show a window indicating the current poll being held.
    /// </summary>
    public bool PollDialog { get; set; } = true;

    /// <summary>
    ///     Whether the poll dialog will use a larger font size when displaying options.
    /// </summary>
    public bool LargeText { get; set; }

    /// <summary>
    ///     Whether the poll dialog will animate votes as they come in.
    /// </summary>
    public bool AnimateVotes { get; set; } = true;

    /// <summary>
    ///     Whether the mod will randomly generate polls from the game's event list, including mod events.
    /// </summary>
    public bool RandomPolls { get; set; } = true;

    /// <summary>
    ///     Whether mod will post active polls in chat.
    /// </summary>
    public bool OptionsInChat { get; set; } = true;

    /// <inheritdoc />
    [JsonPropertyName("_VERSION")]
    public int Version { get; set; } = LatestVersion;
}
