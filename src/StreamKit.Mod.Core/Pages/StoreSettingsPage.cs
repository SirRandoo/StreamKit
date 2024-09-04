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
using SirRandoo.UX.Drawers;
using SirRandoo.UX.Extensions;
using SirRandoo.UX.Helpers;
using StreamKit.Mod.Core.Settings;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.Pages;

public class StoreSettingsPage : SettingsPage
{
    private readonly StoreSettings _settings = new();

    private string? _minimumPurchasePriceBuffer;
    private bool _minimumPurchasePriceBufferValid;
    private Vector2 _scrollPosition = Vector2.zero;

    private string? _storeLinkBuffer;
    private bool _storeLinkBufferValid;

    public StoreSettingsPage()
    {
        _storeLinkBuffer = _settings.StoreLink.ToStringNullable();
        _storeLinkBufferValid = !string.IsNullOrEmpty(_storeLinkBuffer);

        _minimumPurchasePriceBuffer = _settings.MinimumPurchasePrice.ToString("N0");
        _minimumPurchasePriceBufferValid = true;
    }

    public override void Draw(Rect region)
    {
        Listing listing = CreateListing(region);

        listing.Begin(region);
        DrawStoreLinkSetting(listing);
        DrawMinimumPurchasePriceSetting(listing);
        DrawPurchaseConfirmationSetting(listing);
        DrawBuildingsPurchasableSetting(listing);
        listing.End();
    }

    private void DrawStoreLinkSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.StoreLink);
        listing.DrawDescription(KitTranslations.StoreLinkDescription);

        if (FieldDrawer.DrawUri(fieldRegion, out Uri? value, ref _storeLinkBuffer, ref _storeLinkBufferValid))
        {
            _settings.StoreLink = value;
        }
    }

    private void DrawMinimumPurchasePriceSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.MinimumPurchasePrice);
        listing.DrawDescription(KitTranslations.MinimumPurchasePriceDescription);

        if (FieldDrawer.DrawNumberField(fieldRegion, out int value, ref _minimumPurchasePriceBuffer, ref _minimumPurchasePriceBufferValid))
        {
            _settings.MinimumPurchasePrice = value;
        }
    }

    private void DrawPurchaseConfirmationSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        LayoutHelper.TrimToIconRect(ref fieldRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PurchaseConfirmations);
        listing.DrawDescription(KitTranslations.PurchaseConfirmationsDescription);

        bool state = _settings.PurchaseConfirmations;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.PurchaseConfirmations = state;
    }

    private void DrawBuildingsPurchasableSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        LayoutHelper.TrimToIconRect(ref fieldRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PurchaseBuildings);
        listing.DrawDescription(KitTranslations.PurchaseBuildingsDescription);

        bool state = _settings.BuildingsPurchasable;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.BuildingsPurchasable = state;
    }
}
