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

namespace StreamKit.Api.UX;

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
        Rect iconRegion = LayoutHelper.IconRect(region.x + region.width - region.height, region.y, region.height, region.height, UiConstants.LineHeight * 0.3f);

        Widgets.DrawLightHighlight(region);

        GUI.color = Color.grey;
        Widgets.DrawBox(region);
        GUI.color = Color.white;

        UiHelper.Label(labelRegion, label);

        GUI.DrawTexture(iconRegion, Icons.AngleDown);

        Widgets.DrawHighlightIfMouseover(region);

        return Widgets.ButtonInvisible(region);
    }

    internal abstract class DropdownDialog<T>(Rect parentRegion, T current, IReadOnlyList<T> allOptions, Action<T> setter)
        : UX.DropdownDialog<T>(parentRegion, current, allOptions, setter)
    {
        /// <inheritdoc />
        protected override float Margin => 5f;

        /// <inheritdoc />
        protected override void SetInitialSizeAndPosition()
        {
            base.SetInitialSizeAndPosition();

            float yPosition = IsReversed ? DropdownRegion.y - ViewHeight : DropdownRegion.y;

            windowRect = new Rect(DropdownRegion.x, yPosition, DropdownRegion.width, ViewHeight + DropdownRegion.height);
            windowRect = windowRect.ExpandedBy(5f);
        }

        /// <inheritdoc />
        public override void DoWindowContents(Rect inRect)
        {
            var dropdownRegion = new Rect(0f, IsReversed ? inRect.height - UiConstants.LineHeight : 0f, inRect.width, UiConstants.LineHeight);
            var dropdownOptionsRegion = new Rect(0f, IsReversed ? 0f : DropdownRegion.height, inRect.width, inRect.height - UiConstants.LineHeight);

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
            if (DrawButton(region, GetItemLabel(CurrentOption)))
            {
                Close();
            }
        }
    }

    private sealed class StringDropdownDialog : DropdownDialog<string>
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

    private sealed class IdentifiableDropdownDialog : DropdownDialog<IIdentifiable>
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
}
