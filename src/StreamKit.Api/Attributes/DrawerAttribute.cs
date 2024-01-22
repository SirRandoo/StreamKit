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

namespace StreamKit.Api.Attributes;

/// <summary>
///     An attribute that's used to specify the drawer to use for a given setting.
/// </summary>
/// <param name="type">
///     A <see cref="ITypeDrawer{T}" /> implementation that'll be used to draw the setting on screen.
/// </param>
/// <remarks>
///     The setting system automatically infers an appropriate drawer based on the type of a given
///     setting. This attribute should only be used when the setting would get matched with a drawer
///     that couldn't properly represent the setting to the user.
/// </remarks>
[AttributeUsage(AttributeTargets.All, Inherited = false)]
public sealed class DrawerAttribute(Type type) : Attribute
{
    /// <summary>
    ///     A <see cref="ITypeDrawer{T}" /> implementation that'll be used to draw the setting on screen.
    /// </summary>
    public Type Type { get; } = type;
}
