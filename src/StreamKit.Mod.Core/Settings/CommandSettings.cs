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

using System.Text.Json.Serialization;
using StreamKit.Mod.Api;

namespace StreamKit.Mod.Core.Settings;

public class CommandSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the command settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The string all commands must start with in order to be recognized by the mod, and the action
    ///     associated with the command ran.
    /// </summary>
    /// <remarks>
    ///     For optimal user experience, this should generally be a single character.
    /// </remarks>
    public string Prefix { get; set; } = "!";

    /// <summary>
    ///     Whether the mod will use emojis in its command responses.
    /// </summary>
    /// <remarks>
    ///     Due to chat character limits, and verbosity concerns, certain information may be omitted if
    ///     emojis are disabled.
    /// </remarks>
    public bool UseEmojis { get; set; } = true;

    /// <inheritdoc />
    [JsonPropertyName("_VERSION")]
    public int Version { get; set; } = LatestVersion;
}
