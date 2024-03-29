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

using System.Globalization;
using UnityEngine;

namespace StreamKit.Mod.Shared.UX;

/// <summary>
///     A specialized class for drawing ints on screen.
/// </summary>
public class IntTypeDrawer(int currentValue) : TypeDrawer<int>
{
    private string _buffer = currentValue.ToString("N0");
    private bool _bufferValid = true;

    /// <inheritdoc />
    public override void Draw(ref Rect region)
    {
        GUI.color = _bufferValid ? Color.white : Color.red;

        if (!FieldDrawer.DrawTextField(region, _buffer, out string? newContent))
        {
            GUI.color = Color.white;

            return;
        }

        _buffer = newContent;
        GUI.color = Color.white;

        if (int.TryParse(_buffer, NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, NumberFormatInfo.CurrentInfo, out int newValue))
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
