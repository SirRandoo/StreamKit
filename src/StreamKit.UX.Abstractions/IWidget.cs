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

using UnityEngine;

namespace StreamKit.UX.Abstractions
{
    /// <summary>
    ///     Represents an element drawn on screen.
    ///     <br/>
    ///     <br/>
    ///     Widgets can be thought of as elements on a menu, like a checkbox
    ///     or a label. They're the basic building blocks for menus users are
    ///     presented with.
    /// </summary>
    public interface IWidget
    {
        /// <summary>
        ///     The position of the widget on the x axis (horizontally).
        /// </summary>
        float X { get; set; }
        
        /// <summary>
        ///     The position of the widget on the y axis (vertically).
        /// </summary>
        float Y { get; set; }
        
        /// <summary>
        ///     The span of the widget on the x axis (horizontally).
        /// </summary>
        float Width { get; set; }
        
        /// <summary>
        ///     The span of the widget on the y axis (vertically).
        /// </summary>
        float Height { get; set; }

        /// <summary>
        ///     The text that will be displayed to the user when they hover over
        ///     the region this widget is drawn in.
        /// </summary>
        string Tooltip { get; set; }

        /// <summary>
        ///     Draws the widget at the specified region.
        /// </summary>
        /// <param name="region">The region to draw the widget in</param>
        /// <remarks>
        ///     If this widget is managed by another widget, the widget should
        ///     call <see cref="GUI.BeginGroup(Rect)"/> prior to drawing child
        ///     widgets.
        /// </remarks>
        void Draw(Rect region);
    }
}
