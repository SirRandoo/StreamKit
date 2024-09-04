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

using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Integration.Twitch;

public class TwitchComponent() : Component(TwitchId, TwitchName)
{
    private const string TwitchName = "Twitch";
    private const string TwitchId = "streamkit.components.twitch";

    /// <inheritdoc />
    public override ISettingsProvider SettingsProvider { get; } = new SettingsProvider<TwitchSettings>(TwitchId, TwitchName);
}

/// <summary>
///     <para>
///         A class for housing twitch settings.
///     </para>
///     <para>
///         Twitch settings are settings that affect how the mod's internal bot interacts with the
///         Twitch platform.
///     </para>
/// </summary>
public sealed class TwitchSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the twitch settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The OAuth2 token created through Twitch's authentication flow.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    ///     The channel to read chat messages from.
    /// </summary>
    public string? Channel { get; set; }

    /// <summary>
    ///     Whether the mod's internal bot will connect to Twitch automatically.
    /// </summary>
    /// <remarks>
    ///     If <see cref="Token" /> and <see cref="Channel" /> aren't provided, the bot will not
    ///     auto-connect.
    /// </remarks>
    public bool AutoConnect { get; set; }

    /// <summary>
    ///     Whether the mod's internal bot will send a message in chat indicating that it's connected to
    ///     Twitch.
    /// </summary>
    public bool SendConnectionMessage { get; set; }

    /// <inheritdoc />
    public int Version { get; set; } = LatestVersion;
}
