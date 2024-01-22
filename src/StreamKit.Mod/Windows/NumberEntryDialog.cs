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
using System.Diagnostics.CodeAnalysis;
using SirRandoo.CommonLib.Helpers;
using StreamKit.Api;
using UnityEngine;
using Verse;

namespace StreamKit.Mod;

public abstract class ShortEntryDialog<T> : Window where T : struct, IComparable
{
    private readonly T _maximum;
    private readonly T _minimum;
    private readonly Action<T> _setter;

    private string _buffer = null!;
    private bool _bufferValid;
    private T _current;

    public ShortEntryDialog(Action<T> setter, T current, T minimum, T maximum)
    {
        _setter = setter;
        _current = current;
        _minimum = minimum;
        _maximum = maximum;

        _bufferValid = true;

        doCloseX = true;
        closeOnCancel = true;
        doCloseButton = false;
        layer = WindowLayer.Dialog;
    }

    /// <inheritdoc/>
    public override void DoWindowContents(Rect inRect)
    {
        var fieldRegion = new Rect(0f, 0f, inRect.width, Mathf.FloorToInt(Text.SmallFontHeight * 1.25f));
        var rangeRegion = new Rect(0f, fieldRegion.height, inRect.width, Text.SmallFontHeight);
        var buttonRegion = new Rect(0f, rangeRegion.y + rangeRegion.height, inRect.width, Text.SmallFontHeight);

        GUI.BeginGroup(inRect);

        GUI.BeginGroup(fieldRegion);
        DrawEntryField(fieldRegion);
        GUI.EndGroup();

        GUI.BeginGroup(rangeRegion);
        DrawRangeRequirement(rangeRegion.AtZero());
        GUI.EndGroup();

        GUI.BeginGroup(buttonRegion);
        DrawDialogButtons(buttonRegion.AtZero());
        GUI.EndGroup();

        GUI.EndGroup();
    }

    private void DrawEntryField(Rect region)
    {
        GUI.color = Color.red;

        if (UiHelper.TextField(region, _buffer, out string newContent))
        {
            _buffer = newContent;

            if (TryParseNumber(_buffer, out T newValue))
            {
                _current = newValue;
                _bufferValid = true;
            }
            else
            {
                _bufferValid = false;
            }
        }

        if (!_bufferValid)
        {
            UiHelper.FieldIcon(region, TexButton.Info, "The value entered is not a valid number.".MarkNotTranslated());
        }

        GUI.color = Color.white;
    }

    private void DrawRangeRequirement(Rect region)
    {
        if (_bufferValid)
        {
            return;
        }

        UiHelper.Label(
            region,
            $"Must be between {FormatNumber(_minimum)} and {FormatNumber(_maximum)}".MarkNotTranslated(),
            Color.red,
            TextAnchor.MiddleCenter,
            GameFont.Tiny
        );
    }

    private void DrawDialogButtons(Rect region)
    {
        var confirmRegion = new Rect(0f, 0f, Mathf.FloorToInt(region.width * 0.5f) - 5f, region.height);
        var cancelRegion = new Rect(confirmRegion.width + 10f, 0f, confirmRegion.width, region.height);

        if (Widgets.ButtonText(confirmRegion, "Confirm".MarkNotTranslated()))
        {
            _setter(_current);

            Close();
        }

        if (Widgets.ButtonText(cancelRegion, "Cancel".MarkNotTranslated()))
        {
            Close();
        }
    }

    /// <inheritdoc/>
    public override void OnAcceptKeyPressed()
    {
        _setter(_current);

        Close();
    }

    /// <inheritdoc />
    public override void PostOpen()
    {
        _buffer = FormatNumber(_current);
    }

    protected abstract string FormatNumber(T value);
    protected abstract bool TryParseNumber(string value, [NotNullWhen(true)] out T number);
}
