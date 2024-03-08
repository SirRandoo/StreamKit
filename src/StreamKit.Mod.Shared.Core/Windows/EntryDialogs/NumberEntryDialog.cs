using System;
using StreamKit.Mod.Api;
using StreamKit.Mod.Shared.UX;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Shared.Core.Windows;

public abstract class NumberEntryDialog<T> : Window where T : struct, IComparable
{
    private readonly T _maximum;
    private readonly T _minimum;
    private readonly Action<T> _setter;

    private string _buffer = null!;
    private BufferValidityCode _bufferStatus = BufferValidityCode.None;
    private T _current;

    protected NumberEntryDialog(Action<T> setter, T minimum, T maximum)
    {
        _setter = setter;
        _minimum = minimum;
        _maximum = maximum;

        doCloseX = false;
        closeOnCancel = true;
        doCloseButton = false;
        layer = WindowLayer.Dialog;
    }

    /// <inheritdoc />
    public override Vector2 InitialSize => new(300, 140);

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
                statusText = $"Value must be less than or equal to {FormatNumber(_maximum)}".MarkNotTranslated();

                break;
            case BufferValidityCode.TooLow:
                statusColor = Color.red;
                statusText = $"Value must be greater than or equal to {FormatNumber(_minimum)}".MarkNotTranslated();

                break;
            case BufferValidityCode.NaN:
                statusColor = Color.red;
                statusText = "Value is not a valid number.".MarkNotTranslated();

                break;
            default:
                statusColor = new Color(0.72f, 0.72f, 0.72f);
                statusText = $"Enter a value between {FormatNumber(_minimum)} and {FormatNumber(_maximum)}".MarkNotTranslated();

                break;
        }

        if (string.IsNullOrEmpty(statusText))
        {
            return;
        }

        LabelDrawer.Draw(region, statusText, statusColor, TextAnchor.UpperCenter, GameFont.Tiny);
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

    /// <inheritdoc />
    public override void OnAcceptKeyPressed()
    {
        _setter(_current);

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

    private enum BufferValidityCode { None, TooHigh, TooLow, NaN }
}
