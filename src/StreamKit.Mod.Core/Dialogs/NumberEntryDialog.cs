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

using System;
using JetBrains.Annotations;
using StreamKit.UX;
using StreamKit.UX.Drawers;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.Dialogs;

[PublicAPI]
public abstract class NumberEntryDialog<T> : Window where T : struct, IComparable
{
    private readonly T _maximum;
    private readonly T _minimum;

    private string _buffer = null!;
    private BufferValidityCode _bufferStatus = BufferValidityCode.None;
    private T _current;

    protected NumberEntryDialog(T minimum, T maximum)
    {
        _minimum = minimum;
        _maximum = maximum;

        doCloseX = false;
        closeOnCancel = true;
        doCloseButton = false;
        layer = WindowLayer.Dialog;
    }

    /// <inheritdoc />
    public override Vector2 InitialSize => new(300, 140);

    public event EventHandler<T> NumberEntered = null!;

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        var fieldRegion = new Rect(0f, 0f, inRect.width, UiConstants.LineHeight);
        var rangeRegion = new Rect(0f, fieldRegion.height, inRect.width, UiConstants.LineHeight);
        var buttonRegion = new Rect(0f, rangeRegion.y + rangeRegion.height, inRect.width, UiConstants.LineHeight);

        GUI.BeginGroup(inRect);

        GUI.BeginGroup(fieldRegion);
        DrawEntryField(fieldRegion);
        GUI.EndGroup();

        GUI.BeginGroup(rangeRegion);
        DrawValidityStatus(rangeRegion.AtZero());
        GUI.EndGroup();

        GUI.BeginGroup(buttonRegion);
        DrawDialogButtons(buttonRegion.AtZero());
        GUI.EndGroup();

        GUI.EndGroup();
    }

    private void DrawEntryField(Rect region)
    {
        GUI.color = _bufferStatus is BufferValidityCode.None ? Color.white : Color.red;

        if (FieldDrawer.DrawTextField(region, _buffer, out string? newContent))
        {
            _buffer = newContent;
            _bufferStatus = GetBufferValidity();
        }

        GUI.color = Color.white;
    }

    private void DrawValidityStatus(Rect region)
    {
        Color statusColor;
        string statusText;

        switch (_bufferStatus)
        {
            case BufferValidityCode.TooHigh:
                statusColor = Color.red;
                statusText = string.Format(KitTranslations.ValueTooHighInputError, FormatNumber(_maximum));

                break;
            case BufferValidityCode.TooLow:
                statusColor = Color.red;
                statusText = string.Format(KitTranslations.ValueTooLowInputError, FormatNumber(_minimum));

                break;
            case BufferValidityCode.NaN:
                statusColor = Color.red;
                statusText = KitTranslations.NaNInputError;

                break;
            default:
                statusColor = new Color(0.72f, 0.72f, 0.72f);
                statusText = string.Format(KitTranslations.ValueOutOfRangeInputError, FormatNumber(_minimum), FormatNumber(_maximum));

                break;
        }

        if (string.IsNullOrEmpty(statusText))
        {
            return;
        }

        LabelDrawer.DrawLabel(region, statusText, statusColor, TextAnchor.UpperCenter, GameFont.Tiny);
    }

    private void DrawDialogButtons(Rect region)
    {
        var confirmRegion = new Rect(0f, 0f, Mathf.FloorToInt(region.width * 0.5f) - 5f, region.height);
        var cancelRegion = new Rect(confirmRegion.width + 10f, 0f, confirmRegion.width, region.height);

        if (Widgets.ButtonText(confirmRegion, KitTranslations.CommonTextConfirm))
        {
            OnNumberEntered(_current);

            Close();
        }

        if (Widgets.ButtonText(cancelRegion, KitTranslations.CommonTextCancel))
        {
            Close();
        }
    }

    /// <inheritdoc />
    public override void OnAcceptKeyPressed()
    {
        OnNumberEntered(_current);

        Close();
    }

    /// <inheritdoc />
    public override void PostOpen()
    {
        _buffer = FormatNumber(_current);
        _bufferStatus = GetBufferValidity();
    }

    private BufferValidityCode GetBufferValidity()
    {
        if (!TryParseNumber(_buffer, out T number))
        {
            return BufferValidityCode.NaN;
        }

        _current = number;

        if (_current.CompareTo(_maximum) > 0)
        {
            return BufferValidityCode.TooHigh;
        }

        return _current.CompareTo(_minimum) < 0 ? BufferValidityCode.TooLow : BufferValidityCode.None;
    }

    protected abstract string FormatNumber(T value);
    protected abstract bool TryParseNumber(string value, out T number);

    protected virtual void OnNumberEntered(T value)
    {
        NumberEntered?.Invoke(this, value);
    }

    private enum BufferValidityCode { None, TooHigh, TooLow, NaN }
}
