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
using System.Collections.Generic;
using SirRandoo.CommonLib.Helpers;
using StreamKit.Data.Abstractions;
using UnityEngine;
using Verse;

namespace StreamKit.Api.UX;

// TODO: Currently "DropdownDialog" is a modified version of DropdownDrawer's DropdownDialog internal class.

public sealed class SearchDrawer<T>
{
    public delegate IReadOnlyList<string> SearchAction(string query);
    private static int ControlId;
    private string _currentQuery;
    private DropdownDialog<T> _dialog;

    private SearchDrawer()
    {
    }

    public void Draw(Rect region)
    {
    }

    public static SearchDrawer<string> CreateInstance()
    {
        var controlId = $"SK_SearchBox_{ControlId.ToString()}";
        ControlId++;

        return new SearchDrawer<string>();
    }
}

/// <summary>
///     An abstraction around a IMGUI text field.
/// </summary>
public sealed class TextField
{
    private int _controlId;
    private string _controlName;
    private TextEditor _editor;
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

public abstract class DropdownDialog<T> : Window
{
    private const int TotalDisplayableOptions = 9;
    private readonly Action<T> _setter;

    private readonly int _totalOptions;
    protected readonly T CurrentOption;
    protected readonly Rect DropdownRegion;
    private Vector2 _scrollPos = new(0f, 0f);
    protected bool IsReversed;
    protected float ViewHeight;

    protected DropdownDialog(Rect parentRegion, T current, IReadOnlyList<T> allOptions, Action<T> setter)
    {
        _setter = setter;
        Options = allOptions;
        CurrentOption = current;
        DropdownRegion = parentRegion;
        _totalOptions = allOptions.Count;

        doCloseX = false;
        drawShadow = false;
        doCloseButton = false;
        layer = WindowLayer.Dialog;

        closeOnClickedOutside = true;
        absorbInputAroundWindow = true;
        forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
    }

    /// <inheritdoc />
    protected override float Margin => 5f;

    public IReadOnlyList<T> Options { get; set; }

    /// <inheritdoc />
    protected override void SetInitialSizeAndPosition()
    {
        float yPosition = IsReversed ? DropdownRegion.y - ViewHeight - 10f : DropdownRegion.y + DropdownRegion.height + 5f;

        windowRect = new Rect(DropdownRegion.x, yPosition, DropdownRegion.width, ViewHeight);
        windowRect = windowRect.ExpandedBy(5f);
    }

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        var dropdownOptionsRegion = new Rect(0f, 0f, inRect.width, inRect.height);

        GUI.BeginGroup(inRect);

        Widgets.DrawLightHighlight(dropdownOptionsRegion);
        Widgets.DrawLightHighlight(dropdownOptionsRegion);
        Widgets.DrawLightHighlight(dropdownOptionsRegion);

        GUI.BeginGroup(dropdownOptionsRegion);
        DrawDropdownOptions(dropdownOptionsRegion.AtZero());
        GUI.EndGroup();

        GUI.EndGroup();
    }

    protected void DrawDropdownOptions(Rect region)
    {
        float viewportWidth = region.width - (_totalOptions > TotalDisplayableOptions ? 16f : 0f);
        var viewport = new Rect(0f, 0f, viewportWidth, UiConstants.LineHeight * _totalOptions);

        GUI.BeginGroup(region);
        _scrollPos = GUI.BeginScrollView(region.AtZero(), _scrollPos, viewport);

        for (var i = 0; i < _totalOptions; i++)
        {
            var lineRegion = new Rect(0f, UiConstants.LineHeight * i, viewportWidth, UiConstants.LineHeight);

            if (!lineRegion.IsVisible(viewport, _scrollPos))
            {
                continue;
            }

            T item = Options[i];
            var textRegion = new Rect(5f, lineRegion.y, lineRegion.width - 10f, lineRegion.height);

            DrawItemLabel(textRegion, item);

            Widgets.DrawHighlightIfMouseover(lineRegion);

            if (AreItemsEqual(item, CurrentOption))
            {
                Widgets.DrawHighlightSelected(lineRegion);
            }

            if (i % 2 == 0)
            {
                GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.44f);

                Widgets.DrawLineHorizontal(lineRegion.x, lineRegion.y, lineRegion.width);
                Widgets.DrawLineHorizontal(lineRegion.x, lineRegion.y + lineRegion.height, lineRegion.width);

                GUI.color = Color.white;
            }

            if (Widgets.ButtonInvisible(lineRegion, false))
            {
                _setter(item);

                Close();
            }
        }

        GUI.EndScrollView();
        GUI.EndGroup();
    }

    /// <inheritdoc />
    public override void PreOpen()
    {
        ViewHeight = UiConstants.LineHeight * (_totalOptions > TotalDisplayableOptions ? TotalDisplayableOptions : _totalOptions);
        IsReversed = DropdownRegion.y + ViewHeight > UI.screenHeight;

        base.PreOpen();
    }

    /// <inheritdoc />
    public override void PostOpen()
    {
        if (Options.Count <= 0)
        {
            Log.Error("Attempted to create a dropdown menu with no options.");

            Close(false);

            return;
        }

        if (CurrentOption != null)
        {
            ScrollToItem(CurrentOption);
        }
    }

    private void ScrollToItem(T item)
    {
        float totalViewHeight = _totalOptions * UiConstants.LineHeight;

        for (var i = 0; i < _totalOptions; i++)
        {
            if (!AreItemsEqual(Options[i], item))
            {
                continue;
            }

            int startingPageItem = i - Mathf.FloorToInt(TotalDisplayableOptions / 2f) - 1;

            _scrollPos = new Vector2(0f, Mathf.Clamp(startingPageItem * UiConstants.LineHeight, 0f, totalViewHeight));
        }
    }

    protected abstract string GetItemLabel(T item);
    protected abstract void DrawItemLabel(Rect region, T item);
    protected abstract bool AreItemsEqual(T item1, T item2);
}

public sealed class StringDropdownDialog : DropdownDialog<string>
{
    /// <inheritdoc />
    public StringDropdownDialog(Rect parentRegion, string current, IReadOnlyList<string> allOptions, Action<string> setter) : base(
        parentRegion,
        current,
        allOptions,
        setter
    )
    {
    }

    /// <inheritdoc />
    protected override string GetItemLabel(string item) => item;

    /// <inheritdoc />
    protected override void DrawItemLabel(Rect region, string item)
    {
        UiHelper.Label(region, item);
    }

    /// <inheritdoc />
    protected override bool AreItemsEqual(string item1, string item2) => string.Equals(item1, item2, StringComparison.InvariantCultureIgnoreCase);
}

public sealed class IdentifiableDropdownDialog : DropdownDialog<IIdentifiable>
{
    /// <inheritdoc />
    public IdentifiableDropdownDialog(Rect parentRegion, IIdentifiable current, IReadOnlyList<IIdentifiable> allOptions, Action<IIdentifiable> setter) : base(
        parentRegion,
        current,
        allOptions,
        setter
    )
    {
    }

    /// <inheritdoc />
    protected override string GetItemLabel(IIdentifiable item) => item.Name;

    /// <inheritdoc />
    protected override void DrawItemLabel(Rect region, IIdentifiable item)
    {
        UiHelper.Label(region, item.Name);
    }

    /// <inheritdoc />
    protected override bool AreItemsEqual(IIdentifiable item1, IIdentifiable item2) => string.Equals(item1.Id, item2.Id, StringComparison.Ordinal);
}
