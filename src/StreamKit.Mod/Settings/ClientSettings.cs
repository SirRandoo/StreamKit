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

using System;
using System.Collections.Generic;
using StreamKit.Api;

namespace StreamKit.Mod;

/// <summary>
///     A class housing client-side settings.
///     <br/>
///     <br/>
///     Client-side settings are settings that can affect only the
///     physical appearance of the mod's displays, change the output of
///     non-essential methods, like whether viewer's name colors are
///     stored and used.
/// </summary>
public class ClientSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the client settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version"/> to
    ///     convert older settings into a newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <inheritdoc/>
    public int Version { get; set; } = LatestVersion;
}

/// <summary>
///     A class for housing store settings.
///     <br/>
///     <br/>
///     Store settings are settings that affect the inventory of the
///     mod's store and any transactions viewers make through the store.
/// </summary>
public class StoreSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the store settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version"/> to
    ///     convert older settings into a newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The minimum amount of points a viewer has to spend before the
    ///     store will "ship" their order.
    /// </summary>
    public int MinimumPurchasePrice { get; set; }

    /// <summary>
    ///     Whether the store will send "receipts" after every purchase.
    /// </summary>
    public bool PurchaseConfirmations { get; set; } = true;

    /// <summary>
    ///     Whether buildable objects are available for purchase within the
    ///     store.
    /// </summary>
    /// <remarks>
    ///     Buildable objects typically represent buildings, such as walls,
    ///     generators, and monuments.
    /// </remarks>
    public bool BuildingsPurchasable { get; set; }

    /// <inheritdoc/>
    public int Version { get; set; } = LatestVersion;
}

/// <summary>
///     A class for housing point settings.
///     <br/>
///     <br/>
///     Point settings are settings that affect how the mod distributes
///     wealth to viewers, either by restricting the amount of wealth
///     given to a certain viewer, or by restricting wealth distribution
///     altogether.
/// </summary>
public class PointSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the point settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version"/> to
    ///     convert older settings into a newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The amount of points viewers will receive they either:
    ///     <ul>
    ///         <li>first join chat</li>
    ///         <li>have their balance reset</li>
    ///     </ul>
    /// </summary>
    public int StartingBalance { get; set; }

    /// <summary>
    ///     Whether the mod is currently distributing wealth to viewers.
    /// </summary>
    public bool IsDistributing { get; set; } = true;

    /// <summary>
    ///     The amount of time that has to pass before viewers are awarded
    ///     wealth.
    /// </summary>
    public TimeSpan RewardInterval { get; set; }

    /// <summary>
    ///     The amount of points viewers will receive when wealth is
    ///     distributed.
    /// </summary>
    public int RewardAmount { get; set; }

    /// <summary>
    ///     Whether viewers must participate in chat in order to receive
    ///     wealth.
    /// </summary>
    public bool ParticipationRequired { get; set; } = true;

    /// <summary>
    ///     The various tiers of point decay that can occur.
    /// </summary>
    public List<PointDecaySettings> PointDecaySettings { get; set; } = new();

    /// <inheritdoc/>
    public int Version { get; set; } = LatestVersion;
}

/// <summary>
///     A class for housing point decay settings.
///     <br/>
///     <br/>
///     Point decay settings are settings that affect how viewer's income
///     will decay.
/// </summary>
public class PointDecaySettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the point decay settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version"/> to
    ///     convert older settings into a newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The period of time that must pass before viewer's income will
    ///     start decaying.
    /// </summary>
    public TimeSpan Period { get; set; }

    /// <summary>
    ///     A percentage indicating the amount of points viewers will lose
    ///     when they aren't actively participating in chat.
    /// </summary>
    public float DecayPercent { get; set; }

    /// <summary>
    ///     A fixed amount indicating the amount of points viewers will lose
    ///     when they aren't actively participating in chat.
    /// </summary>
    /// <remarks>
    ///     Fixed point decays are removed <i>after</i> the result of the
    ///     percentage based decay calculation.
    /// </remarks>
    public int FixedAmount { get; set; }

    /// <inheritdoc/>
    public int Version { get; set; } = LatestVersion;
}

/// <summary>
///     A class for housing karma settings.
///     <br/>
///     <br/>
///     Karma settings are settings that affect how "karma," the mod's
///     short term limiter on bad purchases. This limiter directly
///     affects how much income viewers can accumulate, and optionally,
///     how much wealth viewers are stockpiling.
/// </summary>
public class KarmaSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the karma settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version"/> to
    ///     convert older settings into a newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The amount of karma viewers will start with when they either:
    ///     <ul>
    ///         <li>first join chat</li>
    ///         <li>have their karma reset</li>
    ///     </ul>
    /// </summary>
    public short StartingKarma { get; set; }

    /// <summary>
    ///     The maximum amount of karma that viewers can acquire.
    /// </summary>
    public short MaximumKarma { get; set; }

    /// <summary>
    ///     The minimum amount of karma viewers can obtain.
    /// </summary>
    public short MinimumKarma { get; set; }

    /// <inheritdoc/>
    public int Version { get; set; } = LatestVersion;
}

/// <summary>
///     A class for housing twitch settings.
///     <br/>
///     <br/>
///     Twitch settings are settings that affect how the mod's internal
///     bot interacts with the Twitch platform.
/// </summary>
public class TwitchSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the twitch settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version"/> to
    ///     convert older settings into a newer format.
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
    ///     Whether the mod's internal bot will connect to Twitch
    ///     automatically.
    /// </summary>
    /// <remarks>
    ///     If <see cref="Token"/> and <see cref="Channel"/> aren't provided,
    ///     the bot will not auto-connect.
    /// </remarks>
    public bool AutoConnect { get; set; }

    /// <summary>
    ///     Whether the mod's internal bot will send a message in chat
    ///     indicating that it's connected to Twitch.
    /// </summary>
    public bool SendConnectionMessage { get; set; }

    /// <inheritdoc/>
    public int Version { get; set; } = LatestVersion;
}

/// <summary>
///     A class for housing poll settings.
///     <br/>
///     <br/>
///     Poll settings are settings that affect how the mod tallies votes.
/// </summary>
public class PollSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the voting settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version"/> to
    ///     convert older settings into a newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    /// <summary>
    ///     The total amount of time a poll will be active for.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    ///     The maximum amount of options that can appear in a poll at any
    ///     given time.
    /// </summary>
    public int MaximumOptions { get; set; }

    /// <summary>
    ///     Whether mod will attempt to use the platform's native polling
    ///     instead of using a chat-based poll.
    /// </summary>
    public bool PreferNativePolls { get; set; }

    /// <summary>
    ///     Whether the mod should also show a window indicating the current
    ///     poll being held.
    /// </summary>
    public bool ShowVotingWindow { get; set; }

    /// <inheritdoc/>
    public int Version { get; set; } = LatestVersion;
}
