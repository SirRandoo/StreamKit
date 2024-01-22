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
using SirRandoo.CommonLib.Helpers;
using StreamKit.Api.Extensions;
using UnityEngine;

namespace StreamKit.Api.UX.Drawers;

public class UriTypeDrawer(Uri? value) : TypeDrawer<Uri?>
{
    private string _buffer = value.ToStringNullable();
    private bool _bufferValid = value != null;

    /// <inheritdoc />
    public override void Draw(ref Rect region)
    {
        GUI.color = _bufferValid ? Color.white : Color.red;

        if (!UiHelper.TextField(region, _buffer, out string newValue))
        {
            GUI.color = Color.white;

            return;
        }

        _buffer = newValue;
        GUI.color = Color.white;

        if (Uri.IsWellFormedUriString(_buffer, UriKind.Absolute))
        {
            _bufferValid = true;

            Value = new Uri(_buffer);
        }
        else
        {
            _bufferValid = false;
        }
    }
}
