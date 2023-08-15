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

namespace StreamKit.Platform.Twitch.Helix.Analytics;

public class GameAnalyticsData
{
    /// <summary>
    ///     A unique id that identifies the game the accompanying analytics
    ///     data is for.
    /// </summary>
    public string GameId { get; set; }

    /// <summary>
    ///     A unique url that can be used to download the report.
    /// </summary>
    /// <remarks>
    ///     The url provided is temporary, and will expire after a certain
    ///     amount of time has elapsed. For the exact window you may use the
    ///     url you can refer to Twitch's official documentation at
    ///     https://dev.twitch.tv/docs/api/reference/#get-game-analytics
    /// </remarks>
    public Uri Url { get; set; }

    /// <summary>
    ///     The type of report being presented.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    ///     The reporting window's start and end dates.
    /// </summary>
    public ApiDateRange DateRange { get; set; }
}
