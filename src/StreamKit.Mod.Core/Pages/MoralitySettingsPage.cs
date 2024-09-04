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

using SirRandoo.UX.Drawers;
using SirRandoo.UX.Helpers;
using StreamKit.Mod.Core.Settings;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.Pages;

public class MoralitySettingsPage : SettingsPage
{
    private readonly MoralitySettings _settings = new();
    private string? _karmaRangeMaximumBuffer;
    private bool _karmaRangeMaximumBufferValid;
    private string? _karmaRangeMinimumBuffer;
    private bool _karmaRangeMinimumBufferValid;
    private Vector2 _scrollPosition = Vector2.zero;
    private string? _startingKarmaBuffer;
    private bool _startingKarmaBufferValid;

    public MoralitySettingsPage()
    {
        _startingKarmaBuffer = _settings.StartingKarma.ToString("N0");
        _startingKarmaBufferValid = true;

        _karmaRangeMinimumBuffer = _settings.KarmaRange.Minimum.ToString("N0");
        _karmaRangeMinimumBufferValid = true;

        _karmaRangeMaximumBuffer = _settings.KarmaRange.Maximum.ToString("N0");
        _karmaRangeMaximumBufferValid = true;
    }

    public override void Draw(Rect region)
    {
        Listing listing = CreateListing(region);

        listing.Begin(region);
        DrawKarmaEnabledSetting(listing);

        if (_settings.IsKarmaEnabled)
        {
            DrawStartingKarmaSetting(listing);
            DrawKarmaRangeSetting(listing);
        }

        DrawReputationEnabledSetting(listing);
        listing.End();
    }

    private void DrawKarmaEnabledSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        LayoutHelper.TrimToIconRect(ref fieldRegion);
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.KarmaSystem);
        listing.DrawDescription(KitTranslations.KarmaSystemDescription);

        bool state = _settings.IsKarmaEnabled;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.IsKarmaEnabled = state;
    }

    private void DrawStartingKarmaSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.StartingKarma);
        listing.DrawDescription(KitTranslations.StartingKarmaDescription);

        if (FieldDrawer.DrawNumberField(fieldRegion, out int value, ref _startingKarmaBuffer, ref _startingKarmaBufferValid, short.MinValue, short.MaxValue))
        {
            _settings.StartingKarma = (short)value;
        }
    }

    private void DrawKarmaRangeSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        LabelDrawer.DrawLabel(labelRegion, KitTranslations.KarmaRange);
        listing.DrawDescription(KitTranslations.KarmaRangeTooltip);

        float halvedWidth = fieldRegion.width * 0.5f;
        var minimumRegion = new Rect(fieldRegion.x, fieldRegion.y, halvedWidth - Text.SmallFontHeight * 0.5f, fieldRegion.height);
        var maximumRegion = new Rect(minimumRegion.x + minimumRegion.width + Text.SmallFontHeight, minimumRegion.y, minimumRegion.width, minimumRegion.height);
        Rect rangeIconRegion = LayoutHelper.IconRect(maximumRegion.x - Text.SmallFontHeight, maximumRegion.y, Text.SmallFontHeight, Text.SmallFontHeight);

        LabelDrawer.DrawLabel(rangeIconRegion, "~", DescriptionDrawer.DescriptionTextColor, TextAnchor.LowerCenter);

        if (FieldDrawer.DrawNumberField(minimumRegion, out int newMinimum, ref _karmaRangeMinimumBuffer, ref _karmaRangeMinimumBufferValid, short.MinValue, short.MaxValue))
        {
            _settings.KarmaRange.Minimum = (short)newMinimum;
        }

        if (FieldDrawer.DrawNumberField(maximumRegion, out int newMaximum, ref _karmaRangeMaximumBuffer, ref _karmaRangeMaximumBufferValid, short.MinValue, short.MaxValue))
        {
            _settings.KarmaRange.Maximum = (short)newMaximum;
        }
    }

    private void DrawReputationEnabledSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);
        LayoutHelper.TrimToIconRect(ref fieldRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.ReputationSystem);
        listing.DrawDescription(KitTranslations.ReputationSystemDescription);

        bool state = _settings.IsReputationEnabled;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.IsReputationEnabled = state;
    }
}
