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
using StreamKit.UX.Extensions;
using UnityEngine;
using Verse;

namespace StreamKit.UX.Drawers;

/// <summary>
///     A collection of specialized methods for drawing checkboxes.
/// </summary>
[PublicAPI]
public static class CheckboxDrawer
{
    /// <summary>
    ///     Draws a checkbox.
    /// </summary>
    /// <param name="region">The region to draw the checkbox in</param>
    /// <param name="state">The current state of the checkbox</param>
    /// <returns>Whether the checkbox was clicked</returns>
    public static bool DrawCheckbox(Rect region, ref bool state)
    {
        GUI.color = state ? Color.green : Color.red;
        GUI.DrawTexture(RectExtensions.IconRect(region.x, region.y, region.width, region.height), state ? Icons.Check.Value : Icons.X.Value, ScaleMode.ScaleToFit);
        GUI.color = Color.white;

        if (Widgets.ButtonInvisible(region))
        {
            state = !state;

            return true;
        }

        return false;
    }

    /// <summary>
    ///     Draws a checkbox.
    /// </summary>
    /// <param name="region">The region to draw the checkbox in</param>
    /// <param name="state">The current state of the checkbox</param>
    /// <returns>Whether the checkbox was clicked</returns>
    public static bool DrawCheckbox(Rect region, string label, ref bool state, TextAnchor anchor = TextAnchor.MiddleLeft)
    {
        var copy = new Rect(region) { width = region.width - region.height - 2f };

        LabelDrawer.DrawLabel(copy, label, anchor);

        copy.x = copy.x + copy.width + 2f;
        copy.width = copy.height;

        bool proxy = state;

        DrawCheckbox(copy, ref proxy);

        bool changed = proxy != state;
        state = proxy;

        return changed;
    }
}
