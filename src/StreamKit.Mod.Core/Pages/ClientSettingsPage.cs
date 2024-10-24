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

using StreamKit.Mod.Core.Settings;
using StreamKit.UX.Drawers;
using StreamKit.UX.Extensions;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.Pages;

public class ClientSettingsPage : SettingsPage
{
    private readonly ClientSettings _settings = new();

    public override void Draw(Rect region)
    {
        Listing listing = CreateListing(region);

        listing.Begin(region);
        DrawGizmoPuffSetting(listing);
        listing.End();
    }

    private void DrawGizmoPuffSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.GizmoPuff);
        listing.DrawDescription(KitTranslations.GizmoPuffDescription);

        bool state = _settings.GizmoPuff;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.GizmoPuff = state;
    }
}
