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

using System.Collections.Generic;
using StreamKit.UX.Dialogs;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.UX;

public sealed class SearchDrawer<T>
{
    public delegate IReadOnlyList<string> SearchAction(string query);
    private string _currentQuery = string.Empty;
    private DropdownDialog<T>? _dialog = null;

    private SearchDrawer()
    {
    }

    public void Draw(Rect region)
    {
    }

    public static SearchDrawer<string> CreateInstance()
    {
        var controlId = $"SK_SearchBox_{SearchDrawerData.ControlId.ToString()}";
        SearchDrawerData.ControlId++;

        return new SearchDrawer<string>();
    }
}

internal static class SearchDrawerData
{
    internal static int ControlId;
}

/// <summary>
///     An abstraction around a IMGUI text field.
/// </summary>
public sealed class TextField
{
    private int _controlId;
    private string _controlName = null!;
    private TextEditor _editor = null!;
    private Rect _region;

    /// <summary>
    ///     The raw content of the text field.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    public int CursorPosition
    {
        get => _editor.cursorIndex;
        set => _editor.cursorIndex = value;
    }

    public IntRange SelectionRange
    {
        get
        {
            if (_editor.cursorIndex == _editor.selectIndex)
            {
                return IntRange.zero;
            }

            return _editor.cursorIndex < _editor.selectIndex
                ? new IntRange(_editor.cursorIndex, _editor.selectIndex - _editor.cursorIndex)
                : new IntRange(_editor.selectIndex, _editor.cursorIndex - _editor.selectIndex);
        }
    }

    /// <summary>
    ///     Initialises the text field at the given region.
    /// </summary>
    /// <param name="region">The region to initialise the text field in.</param>
    public void Initialise(Rect region)
    {
        _region = region;
        _controlName = $"SK_SearchField_{region.ToString()}";
        _controlId = GUIUtility.GetControlID(FocusType.Keyboard, region);
        _editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), _controlId);
    }

    /// <summary>
    ///     Draws the text field on screen.
    /// </summary>
    public void Draw()
    {
        GUI.SetNextControlName(_controlName);
        Content = GUI.TextField(_region, Content, Text.CurTextFieldStyle);
    }
}
