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
using UnityEngine;
using Verse;

namespace StreamKit.Api.UX.Drawers;

/// <summary>
///     A generic class for informing developers of a type that isn't supported by the setting system.
/// </summary>
/// <param name="type">The type that's unsupported by the system.</param>
public class UnknownTypeDrawer(Type type) : ITypeDrawer
{
    private readonly string _typeName = type.Name;

    /// <inheritdoc />
    public void Draw(ref Rect region)
    {
        UiHelper.Label(region, _typeName, Color.grey, TextAnchor.MiddleRight, GameFont.Tiny);
    }

    /// <inheritdoc />
    public void Toggle()
    {
    }

    /// <inheritdoc />
    public void Initialise()
    {
    }
}
