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
using System.Globalization;
using NetEscapades.EnumGenerators;
using SirRandoo.CommonLib.Helpers;
using UnityEngine;

namespace StreamKit.Api.UX.Drawers;

// TODO: Consider allowing more fine grain control of time spans to users.

public class TimeSpanDrawer : TypeDrawer<TimeSpan>
{
    private readonly string[] _stringUnits = TimeUnitExtensions.GetNames();
    private readonly TimeUnit[] _units = TimeUnitExtensions.GetValues();

    private string _buffer;
    private bool _bufferValid;
    private TimeUnit _currentUnit;

    public TimeSpanDrawer(TimeSpan currentValue)
    {
        _currentUnit = GetLargestUnit(currentValue);
        _buffer = Stringify(_currentUnit);
        _bufferValid = true;
    }

    /// <inheritdoc />
    public override void Draw(ref Rect region)
    {
        var fieldRegion = new Rect(region.x, region.y, region.width * 0.7f - 5f, region.height);
        var dropdownRegion = new Rect(fieldRegion.x + fieldRegion.width + 10f, fieldRegion.y, region.width - fieldRegion.width - 10f, fieldRegion.height);

        DropdownDrawer.Draw(
            dropdownRegion,
            _currentUnit.ToStringFast(),
            _stringUnits,
            value =>
            {
                _currentUnit = TimeUnitExtensions.TryParse(value, out TimeUnit unit) ? unit : TimeUnit.Seconds;
            }
        );

        GUI.color = _bufferValid ? Color.white : Color.red;
        if (!UiHelper.TextField(fieldRegion, _buffer, out string newValue))
        {
            GUI.color = Color.white;
            return;
        }

        _buffer = newValue;
        GUI.color = Color.white;

        if (double.TryParse(
            _buffer,
            NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
            NumberFormatInfo.CurrentInfo,
            out double result
        ))
        {
            _bufferValid = true;

            UpdateSetting(result);
        }
        else
        {
            _bufferValid = false;
        }
    }

    private void UpdateSetting(double result)
    {
        switch (_currentUnit)
        {
            case TimeUnit.Seconds:
                Value = TimeSpan.FromSeconds(result);

                break;
            case TimeUnit.Minutes:
                Value = TimeSpan.FromMinutes(result);

                break;
            case TimeUnit.Hours:
                Value = TimeSpan.FromHours(result);

                break;
            case TimeUnit.Days:
                Value = TimeSpan.FromDays(result);

                break;
            default:
                throw new InvalidOperationException("The current time unit is unsupported.");
        }
    }

    private static TimeUnit GetLargestUnit(TimeSpan span)
    {
        if (span.TotalDays > 0)
        {
            return TimeUnit.Days;
        }

        if (span.TotalHours > 0)
        {
            return TimeUnit.Hours;
        }

        if (span.TotalMinutes > 0)
        {
            return TimeUnit.Minutes;
        }

        return TimeUnit.Seconds;
    }

    private string Stringify(TimeUnit unit)
    {
        TimeSpan value = Value;

        switch (unit)
        {
            case TimeUnit.Seconds:
                return value.TotalSeconds.ToString("N2");
            case TimeUnit.Minutes:
                return value.TotalMinutes.ToString("N2");
            case TimeUnit.Hours:
                return value.TotalHours.ToString("N2");
            case TimeUnit.Days:
                return value.TotalDays.ToString("N2");
            default:
                throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported time unit specified.");
        }
    }
}

[EnumExtensions] public enum TimeUnit { Seconds, Minutes, Hours, Days }
