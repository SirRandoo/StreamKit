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

using System.Globalization;
using RimWorld;
using SirRandoo.CommonLib.Helpers;
using StreamKit.Api.Extensions;
using UnityEngine;
using Verse;

namespace StreamKit.Api.UX.Drawers;

/// <summary>
///     A specialized class for drawing a <see cref="ShortRange" /> on screen.
/// </summary>
public class ShortRangeTypeDrawer(ShortRange currentValue) : TypeDrawer<ShortRange>
{
    private string _maximumBuffer = currentValue.Maximum.ToString("N0");
    private bool _maximumBufferValid = true;
    private string _minimumBuffer = currentValue.Minimum.ToString("N0");
    private bool _minimumBufferValid = true;

    /// <inheritdoc />
    public override void Draw(ref Rect region)
    {
        var minimumRegion = new Rect(region.x, region.y, region.width * 0.5f - 10f, region.height);
        var maximumRegion = new Rect(minimumRegion.x + minimumRegion.width + 20f, minimumRegion.y, minimumRegion.width, minimumRegion.height);

        ShortRange value = Value;

        DrawField(minimumRegion, ref value.Minimum, ref _minimumBuffer, ref _minimumBufferValid);
        DrawField(maximumRegion, ref value.Maximum, ref _maximumBuffer, ref _maximumBufferValid);

        UiHelper.Label(region, "~", Color.grey, TextAnchor.MiddleCenter, GameFont.Medium);
        UiHelper.Label(RectExtensions.Shift(ref minimumRegion, Direction8Way.South, 1f), "Minimum".MarkNotTranslated(), Color.grey, TextAnchor.UpperCenter, GameFont.Tiny);
        UiHelper.Label(RectExtensions.Shift(ref maximumRegion, Direction8Way.South, 1f), "Maximum".MarkNotTranslated(), Color.grey, TextAnchor.UpperCenter, GameFont.Tiny);

        Value = value;
    }

    private static void DrawField(Rect region, ref short currentValue, ref string buffer, ref bool bufferValid)
    {
        GUI.color = bufferValid ? Color.white : Color.red;

        if (!UiHelper.TextField(region, buffer, out string newValue))
        {
            GUI.color = Color.white;

            return;
        }

        buffer = newValue;
        GUI.color = Color.white;

        if (short.TryParse(
            buffer,
            NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
            NumberFormatInfo.CurrentInfo,
            out short newShortValue
        ))
        {
            currentValue = newShortValue;
            bufferValid = true;
        }
        else
        {
            bufferValid = false;
        }
    }
}
