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

using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace StreamKit.UX.Drawers;

/// <summary>
///     A collection of specialized classes for drawing text.
/// </summary>
[PublicAPI]
public static class LabelDrawer
{
    /// <summary>
    ///     Draws text on screen.
    /// </summary>
    /// <param name="region">The region to draw the text in.</param>
    /// <param name="text">The text to draw on screen.</param>
    /// <param name="anchor">The text alignment of the text within the region of the screen.</param>
    /// <param name="font">The font size of the text.</param>
    public static void DrawLabel(Rect region, string text, TextAnchor anchor = TextAnchor.MiddleLeft, GameFont font = GameFont.Small)
    {
        DrawLabel(region, text, Color.white, anchor, font);
    }

    /// <summary>
    ///     Draws text on screen.
    /// </summary>
    /// <param name="region">The region to draw the text in.</param>
    /// <param name="text">The text to draw on screen.</param>
    /// <param name="textColor">The color of the text being drawn on screen.</param>
    /// <param name="anchor">The text alignment of the text within the region of the screen.</param>
    /// <param name="font">The font size of the text.</param>
    public static void DrawLabel(Rect region, string text, Color textColor, TextAnchor anchor = TextAnchor.MiddleLeft, GameFont font = GameFont.Small)
    {
        TextAnchor previousAnchor = Text.Anchor;
        GameFont previousFont = Text.Font;
        Color previousColor = GUI.color;

        Text.Anchor = anchor;
        Text.Font = font;

        GUI.color = textColor;
        Widgets.Label(region, text);
        GUI.color = previousColor;

        Text.Anchor = previousAnchor;
        Text.Font = previousFont;
    }

    /// <summary>
    ///     Draws text on screen.
    /// </summary>
    /// <param name="listing">The <see cref="Listing" /> to use for laying out content.</param>
    /// <param name="text">The text to draw on screen.</param>
    /// <param name="textColor">The color of the text being drawn on screen.</param>
    /// <param name="anchor">The text alignment of the text within the region of the screen.</param>
    /// <param name="font">The font size of the text.</param>
    public static void DrawLabel(this Listing listing, string text, Color textColor, TextAnchor anchor = TextAnchor.MiddleLeft, GameFont font = GameFont.Small)
    {
        Rect region = listing.GetRect(UiConstants.LineHeight);

        DrawLabel(region, text, textColor, anchor, font);
    }

    /// <summary>
    ///     Draws text on screen.
    /// </summary>
    /// <param name="listing">The <see cref="Listing" /> to use for laying out content.</param>
    /// <param name="text">The text to draw on screen.</param>
    /// <param name="anchor">The text alignment of the text within the region of the screen.</param>
    /// <param name="font">The font size of the text.</param>
    public static void DrawLabel(this Listing listing, string text, TextAnchor anchor = TextAnchor.MiddleLeft, GameFont font = GameFont.Small)
    {
        Rect region = listing.GetRect(UiConstants.LineHeight);

        DrawLabel(region, text, anchor, font);
    }
}
