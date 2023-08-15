// MIT License
// 
// Copyright (c) 2022 SirRandoo
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
///     Represents an object that draws <see cref="IComponent"/> settings
///     on screen.
/// </summary>
public interface ISettingsDrawer
{
    /// <summary>
    ///     The current position of the drawn settings on the X axis
    ///     (horizontally).
    /// </summary>
    float X { get; set; }

    /// <summary>
    ///     The current position of the drawn settings on the Y axis
    ///     (vertically).
    /// </summary>
    float Y { get; set; }

    /// <summary>
    ///     The span of the drawn settings on the X axis (horizontally).
    /// </summary>
    float Width { get; set; }

    /// <summary>
    ///     The span of the drawn settings on the Y axis (vertically).
    /// </summary>
    float Height { get; set; }

    /// <summary>
    ///     Draws the component's settings on screen.
    /// </summary>
    void Draw();

    /// <summary>
    ///     Invalidates the internal state of the drawer.
    /// </summary>
    void Invalidate();
}
