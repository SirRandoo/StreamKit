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
using StreamKit.Shared;
using StreamKit.Shared.Extensions;
using StreamKit.UX.Drawers;
using StreamKit.UX.Extensions;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.Pages;

public class PollSettingsPage : SettingsPage
{
    private readonly PollSettings _settings = new();

    private string? _durationBuffer;
    private bool _durationBufferValid;
    private UnitOfTime _durationTimeUnit;
    private double _lastDurationParseResult;

    private string? _maximumOptionsBuffer;
    private bool _maximumOptionsBufferValid;

    private Vector2 _scrollPosition = Vector2.zero;

    public PollSettingsPage()
    {
        _durationTimeUnit = _settings.Duration.GetLongestTimePeriod();
        _durationBuffer = _settings.Duration.ToString(_durationTimeUnit);
        _durationBufferValid = true;

        _maximumOptionsBuffer = _settings.MaximumOptions.ToString("N0");
        _maximumOptionsBufferValid = true;
    }

    public override void Draw(Rect region)
    {
        Listing listing = CreateListing(region);

        listing.Begin(region);
        DrawDurationSetting(listing);
        DrawMaximumOptionsSetting(listing);
        DrawPreferNativePollsSetting(listing);

        if (!_settings.PreferNativePolls)
        {
            DrawPollDialogSetting(listing);
            DrawOptionsInChatSetting(listing);
        }

        if (_settings is { PollDialog: true, PreferNativePolls: false })
        {
            DrawLargeTextSetting(listing);
            DrawAnimateVotesSetting(listing);
        }

        DrawRandomPollsSetting(listing);

        listing.End();
    }

    private void DrawDurationSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PollDuration);
        listing.DrawDescription(KitTranslations.PollDurationDescription);

        float dropdownWidth = fieldRegion.width * 0.55f;
        var dropdownRegion = new Rect(fieldRegion.x + fieldRegion.width - dropdownWidth, fieldRegion.y, dropdownWidth, fieldRegion.height);
        fieldRegion.width = fieldRegion.width - dropdownWidth - 4f;

        if (FieldDrawer.DrawNumberField(fieldRegion, out double value, ref _durationBuffer!, ref _durationBufferValid))
        {
            _lastDurationParseResult = value;
            _settings.Duration = ConvertToTimeSpan(value, _durationTimeUnit);
        }

        DropdownDrawer.Draw(
            dropdownRegion,
            _durationTimeUnit.ToStringFast(),
            UnitOfTimeNames,
            newUnit =>
            {
                _durationTimeUnit = UnitOfTimeExtensions.TryParse(newUnit, out UnitOfTime unit) ? unit : UnitOfTime.Seconds;
                _settings.Duration = ConvertToTimeSpan(_lastDurationParseResult, _durationTimeUnit);
            }
        );
    }

    private void DrawMaximumOptionsSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.MaximumPollOptions);
        listing.DrawDescription(KitTranslations.MaximumPollOptionsDescription);

        if (FieldDrawer.DrawNumberField(fieldRegion, out int value, ref _maximumOptionsBuffer, ref _maximumOptionsBufferValid, 2))
        {
            _settings.MaximumOptions = value;
        }
    }

    private void DrawPreferNativePollsSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        fieldRegion = fieldRegion.TrimToIconRect();
        IconDrawer.DrawExperimentalIconCutout(ref labelRegion);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.NativePolls);
        listing.DrawDescription(KitTranslations.NativePollsDescription);

        bool state = _settings.PreferNativePolls;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.PreferNativePolls = state;
    }

    private void DrawPollDialogSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.ShowPollDialog);
        listing.DrawDescription(KitTranslations.ShowPollDialogDescription);

        bool state = _settings.PollDialog;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.PollDialog = state;
    }

    private void DrawLargeTextSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.LargePollText);

        listing.DrawDescription(KitTranslations.LargePollTextDescription);

        bool state = _settings.LargeText;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.LargeText = state;
    }

    private void DrawAnimateVotesSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.AnimatedPollVotes);
        listing.DrawDescription(KitTranslations.AnimatedPollVotesDescription);

        bool state = _settings.AnimateVotes;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.AnimateVotes = state;
    }

    private void DrawRandomPollsSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.GenerateRandomPolls);
        listing.DrawDescription(KitTranslations.GenerateRandomPollsDescriptions);

        bool state = _settings.RandomPolls;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.RandomPolls = state;
    }

    private void DrawOptionsInChatSetting(Listing listing)
    {
        (Rect labelRegion, Rect fieldRegion) = listing.Split();
        fieldRegion = fieldRegion.TrimToIconRect();

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.PollOptionsInChat);
        listing.DrawDescription(KitTranslations.PollOptionsInChatDescription);

        bool state = _settings.OptionsInChat;
        CheckboxDrawer.DrawCheckbox(fieldRegion, ref state);
        _settings.OptionsInChat = state;
    }
}
