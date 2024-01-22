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
using System.Collections.Generic;
using SirRandoo.CommonLib.Helpers;
using StreamKit.Data.Abstractions;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.UX;

/// <summary>
///     A specialized class for drawing dropdown menus.
/// </summary>
public static class DropdownDrawer
{
    /// <summary>
    ///     Draws a dropdown at the given region.
    /// </summary>
    /// <param name="region">The region to draw the dropdown in.</param>
    /// <param name="current">
    ///     The current item being displayed in the dropdown display.
    /// </param>
    /// <param name="allOptions">
    ///     A list containing all the available options a user can select.
    /// </param>
    /// <param name="setterFunc">
    ///     An action that's called when the user selects an option in the
    ///     dropdown menu.
    /// </param>
    public static void Draw(Rect region, string current, IReadOnlyList<string> allOptions, Action<string> setterFunc)
    {
        if (DrawButton(region, current))
        {
            Find.WindowStack.Add(new StringDropdownDialog(GetDialogPosition(region), current, allOptions, setterFunc));
        }
    }

    /// <summary>
    ///     Draws a dropdown at the given region.
    /// </summary>
    /// <param name="region">The region to draw the dropdown in.</param>
    /// <param name="current">
    ///     The current item being displayed in the dropdown display.
    /// </param>
    /// <param name="allOptions">
    ///     A list containing all the available options a user can select.
    /// </param>
    /// <param name="setterFunc">
    ///     An action that's called when the user selects an option in the
    ///     dropdown menu.
    /// </param>
    public static void Draw<T>(Rect region, T current, IReadOnlyList<IIdentifiable> allOptions, Action<IIdentifiable> setterFunc) where T : IIdentifiable
    {
        if (DrawButton(region, current.Name))
        {
            Find.WindowStack.Add(new IdentifiableDropdownDialog(GetDialogPosition(region), current, allOptions, setterFunc));
        }
    }

    private static Rect GetDialogPosition(Rect parentRegion) => UI.GUIToScreenRect(parentRegion);

    private static bool DrawButton(Rect region, string label)
    {
        var labelRegion = new Rect(region.x + 5f, region.y, region.width - region.height, region.height);
        Rect iconRegion = LayoutHelper.IconRect(region.x + region.width - region.height, region.y, region.height, region.height);

        Widgets.DrawLightHighlight(region);

        GUI.color = Color.grey;
        Widgets.DrawBox(region);
        GUI.color = Color.white;

        UiHelper.Label(labelRegion, label);
        UiHelper.Label(iconRegion, "v", TextAnchor.MiddleCenter);

        Widgets.DrawHighlightIfMouseover(region);

        return Widgets.ButtonInvisible(region);
    }

    private abstract class DropdownDialog<T> : Window
    {
        private const int TotalDisplayableOptions = 9;
        private readonly IReadOnlyList<T> _allOptions;
        private readonly T _currentOption;
        private readonly Rect _dropdownRegion;
        private readonly Action<T> _setter;

        private readonly int _totalOptions;
        private bool _isReversed;
        private Vector2 _scrollPos = new(0f, 0f);
        private float _viewHeight;

        protected DropdownDialog(Rect parentRegion, T current, IReadOnlyList<T> allOptions, Action<T> setter)
        {
            _setter = setter;
            _currentOption = current;
            _allOptions = allOptions;
            _dropdownRegion = parentRegion;
            _totalOptions = allOptions.Count;

            doCloseX = false;
            drawShadow = false;
            doCloseButton = false;
            layer = WindowLayer.Dialog;

            closeOnClickedOutside = true;
            absorbInputAroundWindow = true;
            forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
        }

        /// <inheritdoc/>
        protected override float Margin => 5f;

        /// <inheritdoc/>
        protected override void SetInitialSizeAndPosition()
        {
            float yPosition = _isReversed ? _dropdownRegion.y - _viewHeight : _dropdownRegion.y;

            windowRect = new Rect(_dropdownRegion.x, yPosition, _dropdownRegion.width, _viewHeight + _dropdownRegion.height);
            windowRect = windowRect.ExpandedBy(5f);
        }

        /// <inheritdoc/>
        public override void DoWindowContents(Rect inRect)
        {
            var dropdownRegion = new Rect(0f, _isReversed ? inRect.height - UiConstants.LineHeight : 0f, inRect.width, UiConstants.LineHeight);
            var dropdownOptionsRegion = new Rect(0f, _isReversed ? 0f : _dropdownRegion.height, inRect.width, inRect.height - UiConstants.LineHeight);

            GUI.BeginGroup(inRect);

            GUI.BeginGroup(dropdownRegion);
            DrawDropdown(dropdownRegion.AtZero());
            GUI.EndGroup();

            Widgets.DrawLightHighlight(dropdownOptionsRegion);
            Widgets.DrawLightHighlight(dropdownOptionsRegion);
            Widgets.DrawLightHighlight(dropdownOptionsRegion);

            GUI.BeginGroup(dropdownOptionsRegion);
            DrawDropdownOptions(dropdownOptionsRegion.AtZero());
            GUI.EndGroup();

            GUI.EndGroup();
        }

        private void DrawDropdown(Rect region)
        {
            if (DrawButton(region, GetItemLabel(_currentOption)))
            {
                Close();
            }
        }

        private void DrawDropdownOptions(Rect region)
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

                T item = _allOptions[i];
                var textRegion = new Rect(5f, lineRegion.y, lineRegion.width - 10f, lineRegion.height);

                DrawItemLabel(textRegion, item);

                Widgets.DrawHighlightIfMouseover(lineRegion);

                if (AreItemsEqual(item, _currentOption))
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

        /// <inheritdoc/>
        public override void PreOpen()
        {
            _viewHeight = UiConstants.LineHeight * (_totalOptions > TotalDisplayableOptions ? TotalDisplayableOptions : _totalOptions);
            _isReversed = _dropdownRegion.y + _viewHeight > UI.screenHeight;

            base.PreOpen();
        }

        /// <inheritdoc/>
        public override void PostOpen()
        {
            if (_allOptions.Count <= 0)
            {
                Log.Error("Attempted to create a dropdown menu with no options.");

                Close(false);

                return;
            }

            if (_currentOption != null)
            {
                ScrollToItem(_currentOption);
            }
        }

        private void ScrollToItem(T item)
        {
            float totalViewHeight = _totalOptions * UiConstants.LineHeight;

            for (var i = 0; i < _totalOptions; i++)
            {
                if (!AreItemsEqual(_allOptions[i], item))
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

    private sealed class StringDropdownDialog : DropdownDialog<string>
    {
        /// <inheritdoc/>
        public StringDropdownDialog(Rect parentRegion, string current, IReadOnlyList<string> allOptions, Action<string> setter) : base(
            parentRegion,
            current,
            allOptions,
            setter
        )
        {
        }

        /// <inheritdoc/>
        protected override string GetItemLabel(string item) => item;

        /// <inheritdoc/>
        protected override void DrawItemLabel(Rect region, string item)
        {
            UiHelper.Label(region, item);
        }

        /// <inheritdoc/>
        protected override bool AreItemsEqual(string item1, string item2) => string.Equals(item1, item2, StringComparison.InvariantCultureIgnoreCase);
    }

    private sealed class IdentifiableDropdownDialog : DropdownDialog<IIdentifiable>
    {
        /// <inheritdoc/>
        public IdentifiableDropdownDialog(Rect parentRegion, IIdentifiable current, IReadOnlyList<IIdentifiable> allOptions, Action<IIdentifiable> setter) : base(
            parentRegion,
            current,
            allOptions,
            setter
        )
        {
        }

        /// <inheritdoc/>
        protected override string GetItemLabel(IIdentifiable item) => item.Name;

        /// <inheritdoc/>
        protected override void DrawItemLabel(Rect region, IIdentifiable item)
        {
            UiHelper.Label(region, item.Name);
        }

        /// <inheritdoc/>
        protected override bool AreItemsEqual(IIdentifiable item1, IIdentifiable item2) => string.Equals(item1.Id, item2.Id, StringComparison.Ordinal);
    }
}
