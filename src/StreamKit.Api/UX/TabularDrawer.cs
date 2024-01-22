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
using SirRandoo.CommonLib.Enums;
using SirRandoo.CommonLib.Helpers;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.UX;

// TODO: This class should be reorganized into smaller, more dedicated methods for common operations within a tabular screen.

/// <summary>
///     A serialized class for drawing tabbed content on screen.
/// </summary>
/// <remarks>
///     Developers wanting tabbed content should use the <see cref="TabularDrawer.Builder" /> class to
///     instantiate a new tabular drawer.
/// </remarks>
public sealed class TabularDrawer
{
    public delegate void ContentDrawer(Rect region);
    private int _currentPage = 1;
    private float _foregroundProgress;
    private float _highlightProgress;
    private Tab? _lastTab;
    private float _maxHeight;
    private float _maxWidth;
    private Rect _previousRegion = Rect.zero;

    private IReadOnlyList<Tab> _tabs = [];

    private TabularDrawer()
    {
    }

    public Tab? CurrentTab { get; private set; }

    public void Draw(Rect region)
    {
        if (_previousRegion != region)
        {
            _previousRegion = region;
            RecalculateLayout();
        }

        GUI.color = new Color(0.46f, 0.49f, 0.5f);
        Widgets.DrawLightHighlight(region.AtZero());
        GUI.color = Color.white;

        Rect barRegion = DrawHorizontal(region);
        var contentRegion = new Rect(region.x, region.y + barRegion.height, region.width, region.height - barRegion.height);

        if (CurrentTab != null)
        {
            Widgets.DrawLightHighlight(contentRegion);
        }

        Rect innerContentRegion = contentRegion.ContractedBy(16f);
        GUI.BeginGroup(innerContentRegion);

        if (CurrentTab == null)
        {
            GUI.EndGroup();

            return;
        }

        CurrentTab.ContentDrawer?.Invoke(innerContentRegion.AtZero());
        GUI.EndGroup();
    }

    private Rect DrawHorizontal(Rect region)
    {
        float height = Mathf.CeilToInt(Text.LineHeight * 1.5f);
        var tabBarRegion = new Rect(region.x, region.y, region.width, height);

        GUI.BeginGroup(tabBarRegion);
        DrawTabsHorizontally(tabBarRegion);
        GUI.EndGroup();

        return tabBarRegion;
    }

    private void DrawTabsHorizontally(Rect region)
    {
        int tabsPerView = Mathf.FloorToInt(Mathf.CeilToInt(region.width - region.height * 2f) / _maxWidth);
        int totalPages = Mathf.CeilToInt(_tabs.Count / (float)tabsPerView);

        var offset = 0f;
        float usableWidth = region.width;

        if (totalPages > 1)
        {
            var previousPageRegion = new Rect(0f, 0f, region.height, region.height);
            var nextPageRegion = new Rect(region.width - region.height, 0f, region.height, region.height);

            DrawHorizontalNavigation(totalPages, previousPageRegion, nextPageRegion);

            offset = previousPageRegion.width;
            usableWidth -= previousPageRegion.width + nextPageRegion.width;
        }

        var tabBarRegion = new Rect(offset, region.y, usableWidth, region.height);

        GUI.BeginGroup(tabBarRegion);

        int pageIndex = (_currentPage - 1) * tabsPerView;
        int viewCount = Mathf.Min(_tabs.Count, tabsPerView);
        float width = Mathf.FloorToInt(usableWidth / tabsPerView);

        for (var i = 0; i < viewCount; i++)
        {
            int tabIndex = pageIndex + i;
            Tab tab = _tabs[tabIndex];

            var tabRegion = new Rect(tabBarRegion.x + tabIndex * width, tabBarRegion.y, width, region.height);

            if (tab.Icon == null)
            {
                UiHelper.Label(tabRegion, tab.Label, TextAnchor.MiddleCenter);
            }
            else
            {
                DrawTabContent(tabRegion, tab);
            }

            if (Widgets.ButtonInvisible(tabRegion))
            {
                CurrentTab = tab;
            }

            if (tab == CurrentTab)
            {
                DrawTabHighlight(tabRegion);
            }
        }

        GUI.EndGroup();
    }

    private void DrawHorizontalNavigation(int pages, Rect leftRegion, Rect rightRegion)
    {
        UiHelper.Label(leftRegion, "<", TextAnchor.MiddleCenter);
        UiHelper.Label(rightRegion, ">", TextAnchor.MiddleCenter);

        if (Widgets.ButtonInvisible(leftRegion))
        {
            _currentPage = Mathf.Clamp(_currentPage, 1, pages);
        }

        if (Widgets.ButtonInvisible(rightRegion))
        {
            _currentPage = Mathf.Clamp(_currentPage, 1, pages);
        }
    }

    private void DrawTabHighlight(Rect region)
    {
        if (_lastTab != CurrentTab)
        {
            _highlightProgress = 0f;
            _foregroundProgress = 0f;
            _lastTab = CurrentTab;
        }

        Color cache = GUI.color;

        GUI.color = Color.cyan;
        Vector2 center = region.center;
        float accentDistance = Mathf.FloorToInt((region.x - center.x) * 0.75f);
        _highlightProgress = Mathf.SmoothStep(_highlightProgress, accentDistance, 0.15f);
        Widgets.DrawLineHorizontal(center.x - _highlightProgress, region.y + region.height - 1f, _highlightProgress * 2f);

        GUI.color = cache;

        _foregroundProgress = Mathf.SmoothStep(_foregroundProgress, center.x - region.x, 0.15f);
        var animationRegion = new Rect(center.x, center.y, 0f, 0f);

        Widgets.DrawLightHighlight(animationRegion.ExpandedBy(_foregroundProgress));
    }

    private static void DrawTabContent(Rect region, Tab tab)
    {
        Rect iconRegion;

        switch (tab.Layout)
        {
            case IconLayout.IconAndText:
                iconRegion = LayoutHelper.IconRect(region.x + 2f, region.y + 2f, 16f, 16f);
                var textRegion = new Rect(iconRegion.x + 2f, region.y, region.width - iconRegion.width - 2f, region.height);

                UiHelper.Icon(iconRegion, tab.Icon, Color.white);
                UiHelper.Label(textRegion, tab.Label, TextAnchor.MiddleCenter);

                return;
            case IconLayout.Text:
                UiHelper.Label(region, tab.Label, TextAnchor.MiddleCenter);

                return;
            case IconLayout.Icon:
                Vector2 center = region.center;
                iconRegion = LayoutHelper.IconRect(center.x - 8f, center.y - 8f, 16f, 16f);
                UiHelper.Icon(iconRegion, tab.Icon, Color.white);

                return;
            default:
                UiHelper.Label(region, tab.Label, TextAnchor.MiddleCenter);

                return;
        }
    }

    private void RecalculateLayout()
    {
        GameFont cache = Text.Font;
        Text.Font = GameFont.Small;

        for (var index = 0; index < _tabs.Count; index++)
        {
            Tab tab = _tabs[index];
            float width = Text.CalcSize(tab.Label).x;
            float adjustedWidth = width + 10f;

            if (tab.Icon != null)
            {
                adjustedWidth += 20f;
            }

            if (adjustedWidth > _maxWidth)
            {
                _maxWidth = adjustedWidth;
            }

            float height = Text.CalcHeight(tab.Label, width);
            float adjustedHeight = height + 10f;

            if (adjustedHeight > _maxHeight)
            {
                _maxHeight = adjustedHeight;
            }
        }

        Text.Font = cache;
    }

    public class Builder
    {
        private readonly List<Tab> _tabs = [];

        public Builder WithTab(Action<TabOptions> configurator)
        {
            var options = new TabOptions();

            configurator(options);

            if (string.IsNullOrEmpty(options.Label) && options.Icon == null)
            {
                throw new InvalidOperationException("An icon or label must be when adding a new tab.");
            }

            _tabs.Add(new Tab(options.Label, options.Drawer, options.Tooltip, options.Icon, options.AnchoredAtEnd, options.Layout));

            return this;
        }

        public TabularDrawer Build()
        {
            var drawer = new TabularDrawer { _tabs = _tabs.AsReadOnly() };

            if (_tabs.Count > 0)
            {
                drawer.CurrentTab = _tabs[0];
            }

            return drawer;
        }
    }

    public class TabOptions
    {
        public string? Label { get; set; }
        public string? Tooltip { get; set; }
        public ContentDrawer? Drawer { get; set; }
        public Texture2D? Icon { get; set; }
        public bool AnchoredAtEnd { get; set; }
        public IconLayout Layout { get; set; } = IconLayout.IconAndText;
    }

    public sealed record Tab(
        string? Label = null,
        ContentDrawer? ContentDrawer = null,
        string? Tooltip = null,
        Texture2D? Icon = null,
        bool AnchorAtEnd = false,
        IconLayout Layout = IconLayout.IconAndText
    );
}
