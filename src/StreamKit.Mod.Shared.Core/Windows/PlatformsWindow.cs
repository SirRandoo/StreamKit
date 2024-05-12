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
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using RimWorld;
using StreamKit.Common.Data.Abstractions;
using StreamKit.Mod.Api;
using StreamKit.Mod.Shared.Extensions;
using StreamKit.Mod.Shared.UX;
using UnityEngine;
using Verse;
using Logger = NLog.Logger;

namespace StreamKit.Mod.Shared.Core.Windows;

public class PlatformsWindow : Window
{
    private const float RowSplitPercentage = 0.35f;
    private static readonly Logger Logger = KitLogManager.GetLogger("StreamKit.Windows.Platforms");
    private readonly QuickSearchWidget _searchWidget = new();

    private bool _isDebugInstance;
    private Vector2 _listScrollPosition = Vector2.zero;
    private IPlatform? _platform;
    private Dictionary<string, Texture2D> _platformIcons = [];
    private IReadOnlyList<IPlatform> _platforms = null!;

    private IReadOnlyList<IPlatform> _workingList = ImmutableArray<IPlatform>.Empty;

    // TODO: Replace the right branch with the actual platform.
    protected IPlatform? Platform => _isDebugInstance ? _platform : null;

    /// <inheritdoc />
    public override Vector2 InitialSize
    {
        get
        {
            Vector2 initialSize = base.InitialSize;
            initialSize.IncrementX(100);

            return initialSize;
        }
    }

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        var platformListRegion = new Rect(0f, 0f, inRect.width * 0.4f, inRect.height);
        var contentRegion = new Rect(platformListRegion.width + 10f, 0f, inRect.width - platformListRegion.width - 10f, inRect.height);

        GUI.BeginGroup(inRect);

        GUI.BeginGroup(platformListRegion);
        DrawPlatformOverview(GenUI.AtZero(platformListRegion));
        GUI.EndGroup();

        GUI.BeginGroup(contentRegion);

        if (_platform != null)
        {
            DrawPlatformPanel(GenUI.AtZero(contentRegion));
        }

        GUI.EndGroup();

        GUI.EndGroup();
    }

    private void DrawPlatformOverview(Rect region)
    {
        var searchRegion = new Rect(0f, 0f, region.width, UiConstants.LineHeight);
        var listRegion = new Rect(0f, UiConstants.LineHeight + 10f, region.width, region.height - UiConstants.LineHeight - 10f);
        Rect innerListRegion = GenUI.ContractedBy(listRegion, 5f);

        GUI.BeginGroup(searchRegion);
        _searchWidget.OnGUI(GenUI.AtZero(searchRegion), FilterPlatformList);
        GUI.EndGroup();

        Widgets.DrawMenuSection(listRegion);

        GUI.BeginGroup(innerListRegion);
        DrawPlatformList(GenUI.AtZero(innerListRegion));
        GUI.EndGroup();
    }

    private void DrawPlatformList(Rect region)
    {
        int totalPlatforms = _workingList.Count;
        float scrollViewHeight = UiConstants.LineHeight * totalPlatforms;
        var scrollView = new Rect(0f, 0f, region.width - (scrollViewHeight > region.height ? 16f : 0f), scrollViewHeight);

        GUI.BeginGroup(region);
        _listScrollPosition = GUI.BeginScrollView(region, _listScrollPosition, scrollView);

        for (var index = 0; index < totalPlatforms; index++)
        {
            var lineRegion = new Rect(0f, UiConstants.LineHeight * index, scrollView.width, UiConstants.LineHeight);

            if (!lineRegion.IsVisible(region, _listScrollPosition))
            {
                continue;
            }

            if (index % 2 == 1)
            {
                Widgets.DrawHighlight(lineRegion);
            }

            IPlatform? platform = _workingList[index];

            if (_platform == platform)
            {
                Widgets.DrawHighlightSelected(lineRegion);
            }

            // TODO: Draw platform's icon when platforms are implemented.
            LabelDrawer.Draw(lineRegion, platform.Name);

            if (_platformIcons.TryGetValue(platform.Id, out Texture2D? platformIcon))
            {
                IconDrawer.DrawFieldIcon(lineRegion, platformIcon);
            }

            if (Widgets.ButtonInvisible(lineRegion))
            {
                _platform = platform;
            }

            Widgets.DrawHighlightIfMouseover(lineRegion);
        }

        GUI.EndScrollView();
        GUI.EndGroup();
    }

    private void FilterPlatformList()
    {
        if (string.IsNullOrEmpty(_searchWidget.filter.Text))
        {
            _workingList = _platforms; // TODO: Replace this with the actual platform list.
        }

        var copy = new List<IPlatform>();
        IReadOnlyList<IPlatform> registrants = _platforms;

        for (var index = 0; index < registrants.Count; index++)
        {
            IPlatform? platform = registrants[index];

            if (string.Compare(platform.Id, _searchWidget.filter.Text, StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                copy.Add(platform);
            }
            else if (platform == _platform)
            {
                _platform = null;
            }
        }

        _workingList = copy.ToImmutableList();
    }

    private void DrawPlatformPanel(Rect region)
    {
    }

    /// <inheritdoc />
    public override void PreOpen()
    {
        base.PreOpen();

        if (Platform is not null)
        {
            _workingList = _platforms;

            return;
        }

        Logger.Warn("Closing window as no platforms are registered. If this happens repeatedly, you should ask for help from the developer(s).");

        Close(false);
    }

#if DEBUG
    public static PlatformsWindow CreateDebugInstance()
    {
        Logger.Info("Creating debug instance of the 'platforms window'...");

        var iconMap = new Dictionary<string, Texture2D>();
        IList<IPlatform> platforms = PseudoDataGenerator.Platforms.AllRegistrants;

        for (var index = 0; index < platforms.Count; index++)
        {
            IPlatform platform = platforms[index];

            iconMap.Add(platform.Id, GetIconFor(platform.Id));
        }

        return new PlatformsWindow
        {
            _isDebugInstance = true,
            _workingList = new ReadOnlyCollection<IPlatform>(platforms),
            _platforms = new ReadOnlyCollection<IPlatform>(platforms),
            _platformIcons = iconMap
        };
    }

    private static Texture2D GetIconFor(string platformId)
    {
        Logger.Info("Locating icon for {PlatformId}...", platformId);

        if (platformId.StartsWith("sirrandoo.platforms.twitch", StringComparison.InvariantCulture))
        {
            return ContentFinder<Texture2D>.Get("Icons/Brands/Twitch");
        }

        if (platformId.StartsWith("sirrandoo.platforms.kick", StringComparison.InvariantCulture))
        {
            return ContentFinder<Texture2D>.Get("Icons/Brands/Kick");
        }

        if (platformId.StartsWith("sirrandoo.platforms.trovo", StringComparison.InvariantCulture))
        {
            return ContentFinder<Texture2D>.Get("Icons/Brands/Trovo");
        }

        return Texture2D.redTexture;
    }
#endif
}
