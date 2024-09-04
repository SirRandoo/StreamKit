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
using SirRandoo.UX.Dialogs;
using SirRandoo.UX.Drawers;
using StreamKit.Common.Data.Abstractions;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.UX;

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
    public static void Draw<T>(Rect region, T current, IReadOnlyList<IIdentifiable> allOptions, Action<IIdentifiable> setterFunc) where T : IIdentifiable
    {
        if (SirRandoo.UX.Drawers.DropdownDrawer.DrawButton(region, current.Name))
        {
            Find.WindowStack.Add(new IdentifiableDropdownDialog(UI.GUIToScreenRect(region), current, allOptions, setterFunc));
        }
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
            LabelDrawer.DrawLabel(region, item.Name);
        }

        /// <inheritdoc />
        protected override bool AreItemsEqual(IIdentifiable item1, IIdentifiable item2) => string.Equals(item1.Id, item2.Id, StringComparison.Ordinal);
    }
}
