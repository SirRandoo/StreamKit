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
using System.Linq;
using StreamKit.Mod.Core.Settings;
using StreamKit.Shared;
using StreamKit.Shared.Extensions;
using StreamKit.UX;
using StreamKit.UX.Drawers;
using StreamKit.UX.Extensions;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.Pages;

public class PointSettingsPage : SettingsPage
{
    private readonly PointSettings _settings;
    private double _lastRewardIntervalParseResult;
    private Vector2 _pointDecayScrollPosition = Vector2.zero;
    private List<PointDecayViewModel> _pointDecayViewModels;

    private string? _rewardAmountBuffer;
    private bool _rewardAmountBufferValid;

    private string? _rewardIntervalBuffer;
    private bool _rewardIntervalBufferValid;
    private UnitOfTime _rewardIntervalTimeUnit;
    private Vector2 _scrollPosition = Vector2.zero;

    private string? _startingBalanceBuffer;
    private bool _startingBalanceBufferValid;

    private PointSettingsPage(PointSettings settings)
    {
        _settings = settings;

        _pointDecayViewModels = _settings.PointDecaySettings.Select(s => new PointDecayViewModel(s)).ToList();

        _startingBalanceBuffer = settings.StartingBalance.ToString("N0");
        _startingBalanceBufferValid = true;

        _rewardIntervalTimeUnit = settings.RewardInterval.GetLongestTimePeriod();
        _rewardIntervalBuffer = settings.RewardInterval.ToString(_rewardIntervalTimeUnit);
        _rewardIntervalBufferValid = true;

        _rewardAmountBuffer = settings.RewardAmount.ToString("N0");
        _rewardAmountBufferValid = true;
    }

    public override void Draw(Rect region)
    {
        Listing listing = CreateListing(region);

        listing.Begin(region);
        DrawInfinitePointsSetting(listing);
        DrawStartingBalanceSetting(listing);
        DrawIsDistributingSetting(listing);

        if (!_settings.IsDistributing)
        {
            listing.End();

            return;
        }

        DrawRewardIntervalSetting(listing);
        DrawRewardAmountSetting(listing);
        DrawParticipationRequiredSetting(listing);
        DrawHasPointDecaySetting(listing);

        if (_settings.HasPointDecay)
        {
            DrawPointDecaySetting(listing);
        }

        DrawHasPointTiersSetting(listing);
        DrawPointTiersSettings(listing);
        DrawPointRewardSetting(listing);
        DrawPointRewardsSettings(listing);

        listing.End();
    }

    private void DrawInfinitePointsSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.InfinitePoints);
        listing.DrawDescription(KitTranslations.InfinitePointsDescription);

        bool state = _settings.InfinitePoints;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.InfinitePoints = state;
    }

    private void DrawIsDistributingSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.DistributePoints);
        listing.DrawDescription(KitTranslations.DistributePointsDescription);

        bool state = _settings.IsDistributing;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.IsDistributing = state;
    }

    private void DrawStartingBalanceSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.StartingBalance);
        listing.DrawDescription(KitTranslations.StartingBalanceDescription);

        if (FieldDrawer.DrawNumberField(fieldRegion, out int value, ref _startingBalanceBuffer, ref _startingBalanceBufferValid))
        {
            _settings.StartingBalance = value;
        }
    }

    private void DrawRewardIntervalSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PointRewardInterval);
        listing.DrawDescription(KitTranslations.PointRewardIntervalDescription);

        float dropdownWidth = fieldRegion.width * 0.55f;
        var dropdownRegion = new Rect(fieldRegion.x + fieldRegion.width - dropdownWidth, fieldRegion.y, dropdownWidth, fieldRegion.height);
        fieldRegion.width = fieldRegion.width - dropdownWidth - 4f;

        if (FieldDrawer.DrawNumberField(fieldRegion, out double value, ref _rewardIntervalBuffer!, ref _rewardIntervalBufferValid))
        {
            _lastRewardIntervalParseResult = value;
            _settings.RewardInterval = ConvertToTimeSpan(value, _rewardIntervalTimeUnit);
        }

        DropdownDrawer.Draw(
            dropdownRegion,
            _rewardIntervalTimeUnit.ToStringFast(),
            UnitOfTimeNames,
            newUnit =>
            {
                _rewardIntervalTimeUnit = UnitOfTimeExtensions.TryParse(newUnit, out UnitOfTime unit) ? unit : UnitOfTime.Seconds;
                _settings.RewardInterval = ConvertToTimeSpan(_lastRewardIntervalParseResult, _rewardIntervalTimeUnit);
            }
        );
    }

    private void DrawRewardAmountSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PointRewardAmount);
        listing.DrawDescription(KitTranslations.PointRewardAmountDescription);

        if (FieldDrawer.DrawNumberField(fieldRegion, out int value, ref _rewardAmountBuffer, ref _rewardAmountBufferValid))
        {
            _settings.RewardAmount = value;
        }
    }

    private void DrawParticipationRequiredSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.RequireParticipation);
        listing.DrawDescription(KitTranslations.RequireParticipationDescription);

        bool state = _settings.ParticipationRequired;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.ParticipationRequired = state;
    }

    private void DrawHasPointDecaySetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PointDecay);
        listing.DrawDescription(KitTranslations.PointDecayDescription);

        bool state = _settings.HasPointDecay;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.HasPointDecay = state;
    }

    private void DrawPointDecaySetting(Listing listing)
    {
        Rect region = listing.GetRect(PanelLineSpan * Text.SmallFontHeight);
        region = region.ContractedBy(10f);

        var scrollView = new Rect(0f, 0f, region.width - 16f, region.height);
        _pointDecayScrollPosition = GUI.BeginScrollView(region, _pointDecayScrollPosition, scrollView, false, true);

        var y = 0f;

        for (var i = 0; i < _pointDecayViewModels.Count; i++)
        {
            PointDecayViewModel decaySettings = _pointDecayViewModels[i];

            var lineRegion = new Rect(0f, y, scrollView.width, UiConstants.LineHeight);
            y += UiConstants.LineHeight;

            if (!lineRegion.IsVisible(region.AtZero(), _pointDecayScrollPosition))
            {
                continue;
            }

            DropdownDrawer.DrawButton(lineRegion, decaySettings.Name);
            var nameRegion = new Rect(lineRegion.x, lineRegion.y, decaySettings.NameWidth, lineRegion.height);
            Rect renameRegion = RectExtensions.IconRect(lineRegion.x + decaySettings.NameWidth + 5f, lineRegion.y, lineRegion.height, lineRegion.height, 6f);

            if (decaySettings.IsNameEditorOpen)
            {
                float renameFieldWidth = lineRegion.width * 0.25f;
                renameRegion.x = lineRegion.x + renameFieldWidth + 5f;
                nameRegion.width = renameFieldWidth;
            }

            if (Widgets.ButtonImage(renameRegion, decaySettings.IsNameEditorOpen ? Icons.Check.Value : Icons.PenToSquare.Value))
            {
                decaySettings.IsNameEditorOpen = !decaySettings.IsNameEditorOpen;
            }

            if (Widgets.ButtonInvisible(lineRegion))
            {
                decaySettings.AccordionExpanded = !decaySettings.AccordionExpanded;
            }

            if (FieldDrawer.DrawTextField(nameRegion, decaySettings.Name, out string? newValue))
            {
                decaySettings.Name = newValue;
                decaySettings.NameWidth = Text.CalcSize(newValue).x;
            }

            if (!decaySettings.AccordionExpanded)
            {
                continue;
            }

            var contentRegion = new Rect(0f, y, scrollView.width, UiConstants.LineHeight * (PanelLineSpan - 1));
            y += contentRegion.height;

            GUI.BeginGroup(contentRegion);
            DrawPointDecayAccordion(contentRegion.AtZero(), decaySettings);
            GUI.EndGroup();
        }

        GUI.EndScrollView();
    }

    private static void DrawPointDecayAccordion(Rect region, PointDecayViewModel settings)
    {
        Listing listing = CreateListing(region);

        listing.Begin(region);
        DrawPointDecayPeriodSetting(listing, settings);
        DrawPointDecayPercentSetting(listing, settings);
        DrawPointDecayFixedAmountSetting(listing, settings);
        listing.End();
    }

    private static void DrawPointDecayPeriodSetting(Listing listing, PointDecayViewModel settings)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);

        float dropdownWidth = fieldRegion.width * 0.55f;
        var dropdownRegion = new Rect(fieldRegion.x + fieldRegion.width - dropdownWidth, fieldRegion.y, dropdownWidth, fieldRegion.height);
        fieldRegion.width = fieldRegion.width - dropdownWidth - 4f;

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PointDecayPeriod);
        listing.DrawDescription(KitTranslations.PointDecayPeriodDescription);

        string? buffer = settings.PeriodBuffer;
        bool bufferValid = settings.PeriodBufferValid;

        if (FieldDrawer.DrawNumberField(fieldRegion, out double value, ref buffer!, ref bufferValid))
        {
            settings.LastPeriodParseResult = value;
            settings.Period = ConvertToTimeSpan(value, settings.PeriodTimeUnit);
        }

        settings.PeriodBuffer = buffer;
        settings.PeriodBufferValid = bufferValid;

        DropdownDrawer.Draw(
            dropdownRegion,
            settings.PeriodTimeUnit.ToStringFast(),
            UnitOfTimeNames,
            newUnit =>
            {
                settings.PeriodTimeUnit = UnitOfTimeExtensions.TryParse(newUnit, out UnitOfTime unit) ? unit : UnitOfTime.Seconds;
                settings.Period = ConvertToTimeSpan(settings.LastPeriodParseResult, settings.PeriodTimeUnit);
            }
        );
    }

    private static void DrawPointDecayPercentSetting(Listing listing, PointDecayViewModel settings)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.DecayPercentage);
        listing.DrawDescription(KitTranslations.DecayPercentageDescription);

        string? buffer = settings.DecayPercentBuffer;
        bool bufferValid = settings.DecayPercentBufferValid;

        if (FieldDrawer.DrawNumberField(fieldRegion, out float value, ref buffer!, ref bufferValid, 0f, 100f))
        {
            settings.DecayPercent = value;
        }

        settings.DecayPercentBuffer = buffer;
        settings.DecayPercentBufferValid = bufferValid;
    }

    private static void DrawPointDecayFixedAmountSetting(Listing listing, PointDecayViewModel settings)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.FixedPointDecayAmount);
        listing.DrawDescription(KitTranslations.FixedPointDecayAmountDescription);

        string? buffer = settings.FixedAmountBuffer;
        bool bufferValid = settings.FixedAmountBufferValid;

        if (FieldDrawer.DrawNumberField(fieldRegion, out int value, ref buffer!, ref bufferValid, 0))
        {
            settings.FixedAmount = value;
        }

        settings.FixedAmountBuffer = buffer;
        settings.FixedAmountBufferValid = bufferValid;
    }

    private void DrawHasPointTiersSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PointTiers);
        listing.DrawDescription(KitTranslations.PointTiersDescription);

        bool state = _settings.HasPointTiers;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.HasPointTiers = state;
    }

    private void DrawPointTiersSettings(Listing listing)
    {
    }

    private void DrawPointRewardSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PointRewards);
        listing.DrawDescription(KitTranslations.PointRewardsDescription);

        bool state = _settings.HasRewards;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.HasRewards = state;
    }

    private void DrawPointRewardsSettings(Listing listing)
    {
    }

    public static PointSettingsPage CreateInstance()
    {
        var settings = new PointSettings(); // TODO: Replace with actual settings instance.

        return new PointSettingsPage(settings) { _pointDecayViewModels = settings.PointDecaySettings.Select(PointDecayViewModel.CreateInstance).ToList() };
    }

    private sealed class PointDecayViewModel(PointDecaySettings model)
    {
        public bool IsNameEditorOpen { get; set; }
        public bool AccordionExpanded { get; set; }
        public float NameWidth { get; set; }
        public string? PeriodBuffer { get; set; }
        public bool PeriodBufferValid { get; set; }
        public double LastPeriodParseResult { get; set; }
        public UnitOfTime PeriodTimeUnit { get; set; }
        public string? DecayPercentBuffer { get; set; }
        public bool DecayPercentBufferValid { get; set; }
        public string? FixedAmountBuffer { get; set; }
        public bool FixedAmountBufferValid { get; set; }

        public string Name
        {
            get => model.Name;
            set => model.Name = value;
        }

        public int FixedAmount
        {
            get => model.FixedAmount;
            set => model.FixedAmount = value;
        }

        public float DecayPercent
        {
            get => model.DecayPercent * 100f;
            set => model.DecayPercent = value / 100f;
        }

        public TimeSpan Period
        {
            get => model.Period;
            set => model.Period = value;
        }

        public static PointDecayViewModel CreateInstance(PointDecaySettings settings)
        {
            var instance = new PointDecayViewModel(settings);

            instance.PeriodTimeUnit = settings.Period.GetLongestTimePeriod();
            instance.PeriodBuffer = settings.Period.ToString(instance.PeriodTimeUnit);
            instance.PeriodBufferValid = true;

            instance.DecayPercentBuffer = (settings.DecayPercent * 100f).ToString("N2");
            instance.DecayPercentBufferValid = true;

            instance.FixedAmountBuffer = settings.FixedAmount.ToString("N2");
            instance.FixedAmountBufferValid = true;

            instance.NameWidth = Text.CalcSize(settings.Name).x;

            return instance;
        }
    }
}
