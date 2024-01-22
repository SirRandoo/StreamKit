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
using SirRandoo.CommonLib.Helpers;
using UnityEngine;

namespace StreamKit.Api.UX.Drawers;

/// <summary>
///     A specialized class for drawing floats on screen.
/// </summary>
public class FloatTypeDrawer(float currentValue) : TypeDrawer<float>
{
    private string _buffer = currentValue.ToString("F");
    private bool _bufferValid = true;

    /// <inheritdoc />
    public override void Draw(ref Rect region)
    {
        GUI.color = _bufferValid ? Color.white : Color.red;

        if (!UiHelper.TextField(region, _buffer, out string newContent))
        {
            GUI.color = Color.white;

            return;
        }

        _buffer = newContent;
        GUI.color = Color.white;

        if (float.TryParse(
            _buffer,
            NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
            NumberFormatInfo.CurrentInfo,
            out float newValue
        ))
        {
            _bufferValid = true;

            Value = newValue;
        }
        else
        {
            _bufferValid = false;
        }
    }
}
