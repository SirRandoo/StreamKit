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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using StreamKit.UX;
using StreamKit.UX.Drawers;
using StreamKit.UX.Extensions;
using UnityEngine;
using Verse;
using IIdentifiable = StreamKit.Shared.Interfaces.IIdentifiable;

namespace StreamKit.Mod.Core.UX;

[PublicAPI]
public abstract partial class TableDrawer<T> where T : IIdentifiable
{
    private Rect[] _columnRegions = [];
    private IReadOnlyList<TableColumn> _columns = [];

    private List<TableEntry> _contents = [];
    private Rect _previousRegion = Rect.zero;
    private IReadOnlyList<Rect> _readOnlyColumnRegions = [];
    private Vector2 _scrollPos = Vector2.zero;
    private float _viewportHeight;

    /// <summary>
    ///     Called once when the table's contents need to be filtered by the query specified.
    /// </summary>
    /// <param name="query">The query to filter the table's contents by.</param>
    public void NotifySearchRequested(string query)
    {
        _viewportHeight = 0f;
        bool queryGiven = !string.IsNullOrEmpty(query);

        for (var i = 0; i < _contents.Count; i++)
        {
            TableEntry row = _contents[i];

            row.Visible = !queryGiven || row.Data.Name.Contains(query);

            if (row.Visible)
            {
                _viewportHeight += UiConstants.LineHeight;
            }
        }
    }

    /// <summary>
    ///     Called once when the table's draw region has been resized.
    /// </summary>
    /// <param name="region">The new region of the table.</param>
    public void NotifyResolutionChanged(Rect region)
    {
        _previousRegion = region;

        var xPosition = 0f;
        float adjustedWidth = region.width - 16f;

        for (var i = 0; i < _columns.Count; i++)
        {
            var columnRegion = new Rect(xPosition, 0f, adjustedWidth * _columns[i].RelativeWidth, UiConstants.LineHeight);

            _columnRegions[i] = columnRegion;
            xPosition += columnRegion.width;
        }

        _readOnlyColumnRegions = new ReadOnlyCollection<Rect>(_columnRegions);
    }

    /// <summary>
    ///     Draws the table as-is in the region given.
    /// </summary>
    /// <param name="region">The region to draw the table in.</param>
    public void Draw(Rect region)
    {
        if (region != _previousRegion)
        {
            NotifyResolutionChanged(region);

            _previousRegion = region;
        }

        var headerRegion = new Rect(0f, 0f, region.width, UiConstants.LineHeight);
        var contentRegion = new Rect(0f, headerRegion.height, region.width, region.height - headerRegion.height);

        GUI.BeginGroup(region);

        GUI.BeginGroup(headerRegion);
        DrawHeaders(headerRegion);
        GUI.EndGroup();

        GUI.BeginGroup(contentRegion);
        DrawContent(contentRegion.AtZero());
        GUI.EndGroup();

        GUI.EndGroup();
    }

    private void DrawHeaders(Rect region)
    {
        GUI.BeginGroup(region);

        for (var i = 0; i < _columns.Count; i++)
        {
            var offset = 0f;
            TableColumn column = _columns[i];
            Rect headerRegion = _readOnlyColumnRegions[i];
            Rect headerDisplayRegion = headerRegion.ContractedBy(4f);

            Widgets.DrawLightHighlight(headerRegion);
            Widgets.DrawLightHighlight(headerRegion);

            if (i % 2 == 1)
            {
                GUI.color = new Color(Color.grey.r, Color.grey.g, Color.grey.b, 0.5f);
                Widgets.DrawLineVertical(headerRegion.x, headerRegion.y + 2f, headerRegion.height - 4f);
                Widgets.DrawLineVertical(headerRegion.x + headerRegion.width, headerRegion.y + 2f, headerRegion.height - 4f);
                GUI.color = Color.white;
            }

            if (column.Icon != null)
            {
                offset = region.height;
                Rect iconRegion = RectExtensions.IconRect(headerDisplayRegion.x, headerDisplayRegion.y, region.height, region.height);

                GUI.DrawTexture(iconRegion, column.Icon);
            }

            if (!string.IsNullOrEmpty(column.Header))
            {
                var labelRegion = new Rect(headerDisplayRegion.x + offset, headerDisplayRegion.y, headerDisplayRegion.width - offset, region.height);

                LabelDrawer.DrawLabel(labelRegion, column.Header!);
            }

            if (column.SortAction == null)
            {
                continue;
            }

            Texture2D sortIcon = column.SortOrder is SortOrder.Descending ? Icons.SortUp.Value : Icons.SortDown.Value;

            Rect sortRegion = RectExtensions.IconRect(
                headerDisplayRegion.x + headerDisplayRegion.width - headerDisplayRegion.height,
                headerDisplayRegion.y,
                headerDisplayRegion.height,
                headerDisplayRegion.height
            );

            GUI.DrawTexture(sortRegion, sortIcon);

            if (Widgets.ButtonInvisible(headerRegion))
            {
                column.SortOrder = column.SortOrder.Invert();

                column.SortAction(column.SortOrder, _contents);
            }
        }

        GUI.EndGroup();
    }

    private void DrawContent(Rect region)
    {
        var viewport = new Rect(0f, 0f, region.width - 16f, _viewportHeight);

        GUI.BeginGroup(region);
        _scrollPos = GUI.BeginScrollView(region, _scrollPos, viewport, false, true);

        var visibleRowCount = 0;

        for (var i = 0; i < _contents.Count; i++)
        {
            TableEntry rowEntry = _contents[i];

            if (!rowEntry.Visible)
            {
                continue;
            }

            var rowRegion = new Rect(0f, visibleRowCount++ * UiConstants.LineHeight, region.width, UiConstants.LineHeight);

            if (!rowRegion.IsVisible(region, _scrollPos))
            {
                continue;
            }

            if (visibleRowCount % 2 == 1)
            {
                Widgets.DrawLightHighlight(rowRegion);
            }

            DrawRowEntry(rowRegion, rowEntry.Data, _readOnlyColumnRegions);
        }

        GUI.EndScrollView();
        GUI.EndGroup();
    }

    /// <summary>
    ///     Called once for every table row that's visible on screen.
    /// </summary>
    /// <param name="region">The region the row is being drawn in.</param>
    /// <param name="data">The data being drawn at the given row.</param>
    /// <param name="columnRegions">
    ///     A collection of regions implementations can directly use to draw data within the row.
    /// </param>
    protected abstract void DrawRowEntry(Rect region, T data, IReadOnlyList<Rect> columnRegions);
}
