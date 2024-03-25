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
using StreamKit.Mod.Shared.Extensions;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Shared.UX;

/// <summary>
///     A serialized class for drawing tabbed content on screen.
/// </summary>
/// <remarks>
///     Developers wanting tabbed content should use the <see cref="TabularDrawer.Builder" />
///     class to instantiate a new tabular drawer.
/// </remarks>
public class TabularDrawer
{
    public delegate void ContentDrawer(Rect region);
    private const int IconSize = 16;
    private const int IconTextPadding = 5;
    private static readonly Color BackgroundColor = new(0.46f, 0.49f, 0.5f);
    private readonly Dictionary<Tab, Vector2> _labelCache = [];
    private int _currentPage = 1;
    private float _foregroundProgress;
    private float _highlightProgress;
    private Tab? _lastTab;
    private float _maxHeight;
    private float _maxWidth;
    private Rect _previousRegion = Rect.zero;

    private IReadOnlyList<Tab> _tabs = [];
    private int _tabsPerView;
    private int _totalPages;

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

        var barRegion = new Rect(0f, 0f, region.width, UiConstants.TabHeight);
        var contentRegion = new Rect(0f, barRegion.height, region.width, region.height - barRegion.height);

        GUI.BeginGroup(region);

        GUI.color = BackgroundColor;
        Widgets.DrawLightHighlight(RectExtensions.AtZero(ref region));
        GUI.color = Color.white;

        GUI.BeginGroup(barRegion);
        DrawTabs(barRegion);
        GUI.EndGroup();

        if (CurrentTab != null)
        {
            Widgets.DrawLightHighlight(contentRegion);
        }

        RectExtensions.ContractedBy(ref contentRegion, 16f);

        GUI.BeginGroup(contentRegion);

        if (CurrentTab == null)
        {
            GUI.EndGroup();

            return;
        }

        CurrentTab.ContentDrawer?.Invoke(RectExtensions.AtZero(ref contentRegion));
        GUI.EndGroup();

        GUI.EndGroup();
    }

    private void DrawTabs(Rect region)
    {
        var tabBarRegion = new Rect(0f, 0f, region.width, region.height);

        if (_totalPages > 1)
        {
            var previousPageRegion = new Rect(0f, 0f, region.height, region.height);
            var nextPageRegion = new Rect(region.width - region.height, 0f, region.height, region.height);

            DrawHorizontalNavigation(previousPageRegion, nextPageRegion);

            tabBarRegion.SetX(previousPageRegion.width);
            tabBarRegion.SetWidth(tabBarRegion.width - (previousPageRegion.width + nextPageRegion.width));
        }

        GUI.BeginGroup(tabBarRegion);

        // Page #1 -> (1 - 1) * 7 -> 0 * 7 -> 0
        // Page #2 -> (2 - 1) * 7 -> 1 * 7 -> 7
        int pageIndex = (_currentPage - 1) * _tabsPerView;
        int viewCount = Mathf.Min(_tabs.Count, _tabsPerView);
        float width = Mathf.FloorToInt(tabBarRegion.width / _tabsPerView);

        for (var i = 0; i < viewCount; i++)
        {
            int tabIndex = pageIndex + i;

            if (tabIndex >= _tabs.Count)
            {
                break;
            }

            Tab tab = _tabs[tabIndex];

            var tabRegion = new Rect(i * width, tabBarRegion.y, width, region.height);

            Widgets.DrawHighlightIfMouseover(tabRegion);
            TooltipHandler.TipRegion(tabRegion, tab.Tooltip);

            if (tab == CurrentTab)
            {
                DrawTabHighlight(tabRegion);
            }

            if (Widgets.ButtonInvisible(tabRegion))
            {
                CurrentTab = tab;
            }

            GUI.BeginGroup(RectExtensions.ContractedBy(ref tabRegion, 5f));
            DrawTabContent(RectExtensions.AtZero(ref tabRegion), tab);
            GUI.EndGroup();
        }

        GUI.EndGroup();
    }

    private void DrawHorizontalNavigation(Rect leftRegion, Rect rightRegion)
    {
        Vector2 leftRegionCenter = leftRegion.center;
        Vector2 rightRegionCenter = rightRegion.center;

        const float halvedIconSize = IconSize * 0.5f;
        var leftIconRegion = new Rect(leftRegionCenter.x - halvedIconSize, leftRegionCenter.y - halvedIconSize, IconSize, IconSize);
        var rightIconRegion = new Rect(rightRegionCenter.x - halvedIconSize, rightRegionCenter.y - halvedIconSize, IconSize, IconSize);

        IconDrawer.DrawIcon(leftIconRegion, _currentPage == 1 ? Icons.AnglesLeft : Icons.AngleLeft, Color.white);
        IconDrawer.DrawIcon(rightIconRegion, _currentPage == _totalPages ? Icons.AnglesRight : Icons.AngleRight, Color.white);

        if (Widgets.ButtonInvisible(leftRegion))
        {
            _currentPage = _currentPage <= 1 ? _totalPages : _currentPage - 1;
        }

        if (Widgets.ButtonInvisible(rightRegion))
        {
            _currentPage = _currentPage >= _totalPages ? 1 : _currentPage + 1;
        }

        Widgets.DrawHighlightIfMouseover(leftRegion);
        Widgets.DrawHighlightIfMouseover(rightRegion);
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

        Widgets.DrawLightHighlight(RectExtensions.ExpandedBy(ref animationRegion, _foregroundProgress));
    }

    private void DrawTabContent(Rect region, Tab tab)
    {
        Rect iconRegion;

        switch (tab.Layout)
        {
            case IconLayout.IconAndText:
                Vector2 center = region.center;
                Vector2 tabSize = _labelCache[tab];

                float contentWidth = tabSize.x - 10f;
                float halvedContentWidth = contentWidth * 0.5f;
                iconRegion = new Rect(center.x - halvedContentWidth, center.y - IconSize * 0.5f, IconSize, IconSize);

                var textRegion = new Rect(
                    iconRegion.x + IconSize + IconTextPadding,
                    center.y - UiConstants.LineHeight * 0.5f,
                    contentWidth - IconSize - IconTextPadding,
                    UiConstants.LineHeight
                );


                IconDrawer.DrawIcon(iconRegion, tab.Icon!, Color.white);
                LabelDrawer.Draw(textRegion, tab.Label!);

                return;
            case IconLayout.Text:
                LabelDrawer.Draw(region, tab.Label!, TextAnchor.MiddleCenter);

                return;
            case IconLayout.Icon:
                iconRegion = LayoutHelper.IconRect(0f, 0f, IconSize, region.height);
                IconDrawer.DrawIcon(iconRegion, tab.Icon!, Color.white);

                return;
            default:
                LabelDrawer.Draw(region, tab.Label!, TextAnchor.MiddleCenter);

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

            if (!_labelCache.TryGetValue(tab, out Vector2 vector))
            {
                Vector2 size = !string.IsNullOrEmpty(tab.Label) ? Text.CalcSize(tab.Label) : new Vector2();

                if (tab.Icon != null)
                {
                    size.IncrementX(IconSize + IconTextPadding);
                }

                size.IncrementX(10f); // Padding

                _labelCache[tab] = vector = size;
            }

            if (vector.x > _maxWidth)
            {
                _maxWidth = vector.x;
            }

            if (vector.y > _maxHeight)
            {
                _maxHeight = vector.y;
            }
        }

        Text.Font = cache;

        int tabBarWidth = Mathf.CeilToInt(_previousRegion.width - UiConstants.TabHeight * 2f);
        _tabsPerView = Mathf.FloorToInt(tabBarWidth / _maxWidth);
        _totalPages = Mathf.CeilToInt(_tabs.Count / (float)_tabsPerView);
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

            _tabs.Add(new Tab(options.Label, options.Drawer, options.Tooltip, options.Icon, options.Layout));

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
        public IconLayout Layout { get; set; } = IconLayout.Text;
    }

    public sealed record Tab(
        string? Label = null,
        ContentDrawer? ContentDrawer = null,
        string? Tooltip = null,
        Texture2D? Icon = null,
        IconLayout Layout = IconLayout.IconAndText
    );
}
