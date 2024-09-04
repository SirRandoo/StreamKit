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

using RimWorld;
using SirRandoo.UX.Drawers;
using SirRandoo.UX.Extensions;
using SirRandoo.UX.Helpers;
using StreamKit.Common.Data.Abstractions;
using StreamKit.Mod.Core.Settings;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.Pages;

public class PawnSettingsPage : SettingsPage
{
    private readonly PawnSettings _settings = new();
    private Vector2 _scrollPosition = Vector2.zero;
    private string? _totalOverworkedPawnsBuffer;
    private bool _totalOverworkedPawnsBufferValid;

    public PawnSettingsPage()
    {
        _totalOverworkedPawnsBuffer = _settings.TotalOverworkedPawns.ToString("N0");
        _totalOverworkedPawnsBufferValid = true;
    }

    public override void Draw(Rect region)
    {
        Listing listing = CreateListing(region);

        listing.Begin(region);
        DrawPawnPoolingSetting(listing);

        if (_settings.Pooling)
        {
            DrawPawnPoolingRestrictionSetting(listing);
        }

        DrawPawnVacationingSetting(listing);

        if (_settings.Vacationing)
        {
            DrawTotalOverworkedPawnsSetting(listing);
            DrawEmergencyWorkCrisisSetting(listing);
        }

        listing.End();
    }

    private void DrawPawnPoolingSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        LayoutHelper.TrimToIconRect(ref fieldRegion);
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PawnPool);
        listing.DrawDescription(KitTranslations.PawnPoolDescription);

        bool state = _settings.Pooling;

        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.Pooling = state;
    }

    private void DrawPawnPoolingRestrictionSetting(Listing listing)
    {
        bool isVip = _settings.PoolingRestrictions.HasFlagFast(UserRoles.Vip);
        bool isModerator = _settings.PoolingRestrictions.HasFlagFast(UserRoles.Moderator);
        bool isSubscriber = _settings.PoolingRestrictions.HasFlagFast(UserRoles.Subscriber);

        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);
        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PawnPoolRestrictions);
        listing.DrawDescription(KitTranslations.PawnPoolRestrictionsDescription);

        fieldRegion.SetHeight(Text.SmallFontHeight);

        CheckboxDrawer.DrawCheckbox(fieldRegion, KitTranslations.CommonTextVip, ref isVip, TextAnchor.MiddleRight);

        CheckboxDrawer.DrawCheckbox(
            RectExtensions.Shift(ref fieldRegion, Direction8Way.South, 0f),
            KitTranslations.CommonTextSubscriber,
            ref isSubscriber,
            TextAnchor.MiddleRight
        );

        CheckboxDrawer.DrawCheckbox(
            RectExtensions.Shift(ref fieldRegion, Direction8Way.South, 0f),
            KitTranslations.CommonTextModerator,
            ref isModerator,
            TextAnchor.MiddleRight
        );

        GUI.color = Color.white;

        var @default = UserRoles.None;

        if (isVip)
        {
            @default |= UserRoles.Vip;
        }

        if (isSubscriber)
        {
            @default |= UserRoles.Subscriber;
        }

        if (isModerator)
        {
            @default |= UserRoles.Moderator;
        }

        _settings.PoolingRestrictions = @default;
    }

    private void DrawPawnVacationingSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        LayoutHelper.TrimToIconRect(ref fieldRegion);
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PawnVacationing);
        listing.DrawDescription(KitTranslations.PawnVacationingDescription);

        bool state = _settings.Vacationing;

        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.Vacationing = state;
    }

    private void DrawTotalOverworkedPawnsSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.OverworkedPawns);
        listing.DrawDescription(KitTranslations.OverworkedPawnsDescription);

        if (FieldDrawer.DrawNumberField(fieldRegion, out int value, ref _totalOverworkedPawnsBuffer, ref _totalOverworkedPawnsBufferValid, 1, short.MaxValue))
        {
            _settings.TotalOverworkedPawns = (short)value;
        }
    }

    private void DrawEmergencyWorkCrisisSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        LayoutHelper.TrimToIconRect(ref fieldRegion);
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.EmergencyWorkCrisis);
        listing.DrawDescription(KitTranslations.EmergencyWorkCrisisDescription);

        bool state = _settings.EmergencyWorkCrisis;

        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.EmergencyWorkCrisis = state;
    }
}
