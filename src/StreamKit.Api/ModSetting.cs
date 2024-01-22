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

namespace StreamKit.Api;

/// <summary>
///     Represents a data class for housing information about a setting for the mod.
/// </summary>
/// <param name="Label">The label for the setting.</param>
/// <param name="Drawer">
///     A <see cref="ITypeDrawer" /> instance that's used to draw the setting's value on screen.
/// </param>
/// <param name="Order">The order of the setting within the menu the setting is being drawn in.</param>
/// <param name="Description">
///     The optional description of the setting used to provide more information about what the setting
///     is to the user.
/// </param>
public record ModSetting(string Label, ITypeDrawer Drawer, int Order, string? Description = null);
