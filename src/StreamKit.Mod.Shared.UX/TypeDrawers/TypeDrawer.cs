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
using StreamKit.Mod.Api;
using UnityEngine;

namespace StreamKit.Mod.Shared.UX;

public abstract class TypeDrawer<T> : ITypeDrawer<T>
{
    /// <inheritdoc />
    public required Func<object> Getter { get; set; }

    /// <inheritdoc />
    public required Action<object?> Setter { get; set; }

    /// <inheritdoc />
    public T? Value
    {
        get => (T?)Getter();
        set => Setter(value);
    }

    /// <inheritdoc />
    public abstract void Draw(ref Rect region);

    /// <inheritdoc />
    public virtual void Toggle()
    {
    }

    /// <inheritdoc />
    public virtual void Initialise()
    {
    }
}
