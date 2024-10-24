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
///     A set of utility methods for drawing boxes on screen.
/// </summary>
[PublicAPI]
public static class BoxDrawer
{
    /// <summary>
    ///     Draws a box on screen.
    /// </summary>
    /// <param name="region">The region of the screen to draw the box in.</param>
    /// <param name="thickness">The number of pixels the box's outline is.</param>
    /// <param name="color">The optional color of the box.</param>
    /// <remarks>
    ///     This is a slightly modified version of <see cref="Widgets.DrawBox(Rect, int)" /> in that it
    ///     takes an optional color as a parameter, but more importantly that the left side of the box
    ///     doesn't have a rounding error in its thickness. The original version from
    ///     <see cref="Widgets" /> can occasionally draw a thicker border on the left at some sizes and
    ///     resolutions since it uses <see cref="Mathf.Ceil" />.
    /// </remarks>
    public static void DrawBox(Rect region, int thickness = 1, Color? color = null)
    {
        var drawRegion = new Rect();
        Color previousColor = GUI.color;

        float thicknessForUiScale = Prefs.UIScale * thickness;
        float finalThickness = Mathf.Floor(thickness - (thicknessForUiScale - Mathf.Ceil(thicknessForUiScale)) / Prefs.UIScale);

        if (color != null)
        {
            GUI.color = color.Value;
        }

        // Draw left side
        drawRegion.x = region.x;
        drawRegion.y = region.y;
        drawRegion.width = finalThickness;
        drawRegion.height = region.height;
        GUI.DrawTexture(drawRegion, BaseContent.WhiteTex);

        // Draw right side
        drawRegion.x = region.x + region.width - 1;
        GUI.DrawTexture(drawRegion, BaseContent.WhiteTex);

        // Draw top side
        drawRegion.x = region.x;
        drawRegion.height = finalThickness;
        drawRegion.width = region.width;
        GUI.DrawTexture(drawRegion, BaseContent.WhiteTex);

        // Draw bottom side
        drawRegion.y = region.y + region.height - 1;
        GUI.DrawTexture(drawRegion, BaseContent.WhiteTex);

        if (color != null)
        {
            GUI.color = previousColor;
        }
    }
}
