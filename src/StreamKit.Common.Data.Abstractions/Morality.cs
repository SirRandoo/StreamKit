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

using System.Diagnostics.CodeAnalysis;
using NetEscapades.EnumGenerators;

namespace StreamKit.Common.Data.Abstractions;

/// <summary>
///     An enum representing the various moral quandaries of purchasing certain products within the
///     flag.
/// </summary>
[EnumExtensions]
[SuppressMessage("ReSharper", "GrammarMistakeInComment")]
public enum Morality
{
    /// <summary>
    ///     Super bad purchases generally refer to products that may negatively impact the colony for
    ///     quadrums -- a unit of time measured in a span of 15 in-game days. In essence, this moral
    ///     generally refers to products that will linger around for extended periods of time that have the
    ///     opportunity to break a colony.
    /// </summary>
    SuperBad = -4,

    /// <summary>
    ///     Represents a really bad purchases which generally refer to products that may negatively impact
    ///     the colony immediately, and have the potential to destroy a significant amount of the colony,
    ///     like mechanoid raids.
    /// </summary>
    ReallyBad = -3,

    /// <summary>
    ///     Represents a very bad purchases which generally refer to products that may negatively impact
    ///     the colony immediately or after a variable amount of time. These products may have a wide range
    ///     of effects that may injure the colony, but are generally considered a non-issue.
    /// </summary>
    VeryBad = -2,

    /// <summary>
    ///     Represents a bad purchase which generally refer to products that may negatively impact the
    ///     colony in a mild enough way, but may be the catalyst for something larger, like the mood
    ///     deficits from vomit rain.
    /// </summary>
    Bad = -1,

    /// <summary>
    ///     Represents a neutral purchase which generally won't impact the colony in a significant way,
    ///     like rain or fog purchases.
    /// </summary>
    Neutral = 0,

    /// <summary>
    ///     Represents a good purchase which generally refers to products that may positively impact the
    ///     colony in a mild enough way and have the potential to be the catalyst for a significant number
    ///     of colony members, like a surgery inspiration.
    /// </summary>
    Good = 1,

    /// <summary>
    ///     Represents a very good purchase which generally refers to products that may positively impact
    ///     the colony immediately or after a variable amount of time. Said products may range from
    ///     mood bonuses to supplies.
    /// </summary>
    VeryGood = 2,

    /// <summary>
    ///     Represents a really good purchase which generally refers to products that may positively impact
    ///     the colony immediately, and have the potential to be a catalyst for a significant number of
    ///     colony members, like a surgery inspiration.
    /// </summary>
    ReallyGood = 3,

    /// <summary>
    ///     Represents a super good purchase which generally refers to products that may positively impact
    ///     the colony for quadrums -- a unit of measurement in time that spans 15 days. In essence, said
    ///     purchase has the ability to save a colony from the brink of death.
    /// </summary>
    SuperGood = 4
}
