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

namespace StreamKit.Platform.Twitch.Helix;

public class BitsLeaderboardData
{
    /// <summary>
    ///     A unique id that identifies the user on the leaderboard.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    ///     The unlocalized name of the user on the leaderboard.
    /// </summary>
    public string UserLogin { get; set; }

    /// <summary>
    ///     The potentially localized name of the user the leaderboard, or
    ///     their login name with varying capitalization.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    ///     The position the user is on the leaderboard.
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    ///     The number of bits the user has cheered thus far.
    /// </summary>
    public int Score { get; set; }
}
