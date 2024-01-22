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

using System.Collections.Immutable;
using RimWorld;
using SirRandoo.CommonLib.Helpers;
using StreamKit.Api;
using StreamKit.Api.UX;
using StreamKit.Api.UX.Tables;
using StreamKit.Data.Abstractions;
using UnityEngine;
using Verse;

namespace StreamKit.Mod;

// TODO: The transaction history dialog currently doesn't show column names.
// TODO: Refunded transactions may require explicit support within the ITransaction interface to provide better information to streamers.
// TODO: The transaction history dialog currently doesn't have icons (outside of the search icon).
// TODO: The transaction history dialog currently doesn't have a way to refund transactions.
// TODO: The transaction history dialog currently doesn't have a way to individually remove transactions, or in bulk.
// TODO: The transaction history dialog currently doesn't have a way to mark transactions as "excluded" from the reputation system.
// TODO: Transactions currently use an enum to describe purchased goods. This will limit 3rd party developer's use of the transaction system.

/// <summary>
///     A dialog for showing the transaction history of a viewer.
/// </summary>
public class TransactionHistoryDialog : Window
{
    private readonly QuickSearchWidget _searchWidget = new();
    private readonly IViewerData _viewer;
    private bool _isSearchExpanded;
    private Vector2 _scrollPos = new(0f, 0f);
    private float _searchOpenTime;
    private TransactionTableDrawer _tableDrawer = null!;
    private readonly ImmutableList<ITransaction> _workingList;

    private TransactionHistoryDialog(IViewerData viewer)
    {
        _viewer = viewer;
        _workingList = viewer.Transactions.ToImmutableList();

        layer = WindowLayer.Dialog;
    }

    /// <inheritdoc />
    public override Vector2 InitialSize => new(600, 500);

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        var headerRegion = new Rect(0f, 0f, inRect.width, UiConstants.LineHeight);
        var contentRegion = new Rect(0f, UiConstants.LineHeight + 5f, inRect.width, inRect.height - UiConstants.LineHeight - 5f);

        GUI.BeginGroup(inRect);

        GUI.BeginGroup(headerRegion);
        DrawHeader(headerRegion);
        GUI.EndGroup();

        GUI.BeginGroup(contentRegion);
        _tableDrawer.Draw(contentRegion.AtZero());

        // DrawTransactions(contentRegion.AtZero());
        GUI.EndGroup();

        GUI.EndGroup();
    }

    private void DrawTransactions(Rect region)
    {
        float viewportHeight = _workingList.Count * UiConstants.LineHeight;
        bool scrollbarVisible = viewportHeight > region.height;
        var viewport = new Rect(0f, 0f, region.width - (scrollbarVisible ? 16f : 0f), viewportHeight);

        GUI.BeginGroup(region);
        _scrollPos = GUI.BeginScrollView(region, _scrollPos, viewport);

        for (var index = 0; index < _workingList.Count; index++)
        {
            var lineRegion = new Rect(0f, index * UiConstants.LineHeight, viewport.width, UiConstants.LineHeight);

            if (!lineRegion.IsVisible(region, _scrollPos))
            {
                continue;
            }

            if (index % 2 == 1)
            {
                Widgets.DrawHighlight(lineRegion);
            }

            ITransaction transaction = _workingList[index];

            var labelRegion = new Rect(5f, lineRegion.y, Mathf.FloorToInt(lineRegion.width * 0.4f) - 5f, lineRegion.height);
            var typeRegion = new Rect(labelRegion.x + labelRegion.width, lineRegion.y, Mathf.FloorToInt(lineRegion.width * 0.25f), lineRegion.height);
            var priceRegion = new Rect(typeRegion.x + typeRegion.width, lineRegion.y, lineRegion.width - labelRegion.width - typeRegion.width, lineRegion.height);

            UiHelper.Label(labelRegion, transaction.Name);

            TooltipHandler.TipRegion(labelRegion, $"Transaction id: {transaction.Id}".MarkNotTranslated());


            UiHelper.Label(typeRegion, transaction.Type.ToStringFast().MarkNotTranslated());


            string prefix = transaction.Refunded ? "+" : string.Empty;
            var stringifiedPrice = transaction.Price.ToString("N0");

            UiHelper.Label(priceRegion, $"{prefix}{stringifiedPrice}");

            TooltipHandler.TipRegion(
                priceRegion,
                $"{_viewer.Name} purchased {transaction.Type.ToStringFast()} for {stringifiedPrice} points on {transaction.PurchasedAt.ToLocalTime():f}".MarkNotTranslated()
            );

            if (transaction.Refunded)
            {
                UiHelper.Label(priceRegion, $"+{stringifiedPrice}");

                TooltipHandler.TipRegion(priceRegion, $"{transaction.Price:N0} was refunded to {_viewer.Name}.".MarkNotTranslated());
            }
            else
            {
                UiHelper.Label(priceRegion, stringifiedPrice);
            }
        }

        GUI.EndScrollView();
        GUI.EndGroup();
    }

    private void DrawHeader(Rect region)
    {
        var titleRegion = new Rect(0f, 0f, Mathf.FloorToInt(region.width * 0.5f), region.height);
        var searchRegion = new Rect(titleRegion.width + 5f, 0f, region.width - titleRegion.width - 5f, region.height);
        Rect searchBtnRegion = LayoutHelper.IconRect(region.width - region.height, 0f, region.height, region.height);

        // TODO: Include the platform icon with the header text.
        UiHelper.Label(titleRegion, $"{_viewer.Name}'s purchase history".MarkNotTranslated());

        if (!_isSearchExpanded)
        {
            GUI.DrawTexture(searchBtnRegion, TexButton.Search);

            if (Widgets.ButtonInvisible(searchBtnRegion))
            {
                _isSearchExpanded = true;
                _searchOpenTime = Time.unscaledTime;
            }
        }

        if (_isSearchExpanded)
        {
            _searchWidget.OnGUI(searchRegion, FilterTransactions);
        }
    }

    private void FilterTransactions() => _tableDrawer.NotifySearchRequested(_searchWidget.filter.Text);

    /// <inheritdoc />
    public override void WindowUpdate()
    {
        base.WindowUpdate();

        TryCollapseSearchWidget();
    }

    private void TryCollapseSearchWidget()
    {
        if (!string.IsNullOrEmpty(_searchWidget.filter.Text))
        {
            return;
        }

        if (_searchOpenTime <= 0)
        {
            return;
        }

        if (Time.unscaledTime - _searchOpenTime >= 10)
        {
            _searchOpenTime = 0f;
            _isSearchExpanded = false;
        }
    }

    /// <summary>
    ///     Creates a new default instance of a transaction history dialog.
    /// </summary>
    /// <param name="viewer">
    ///     The viewer whose transactions history is being drawn.
    /// </param>
    public static TransactionHistoryDialog CreateDefaultInstance(IViewerData viewer) => new(viewer)
    {
        _tableDrawer = TransactionTableDrawer.BuildInstance()
           .WithColumn(
                o =>
                {
                    o.Header = "Name";
                    o.RelativeWidth = 0.333f;
                }
            )
           .WithColumn(
                o =>
                {
                    o.Header = "Type";
                    o.RelativeWidth = 0.333f;
                }
            )
           .WithColumn(
                o =>
                {
                    o.Header = "Amount";
                    o.RelativeWidth = 0.333f;
                }
            )
           .Build(viewer.Transactions)
    };

    /// <summary>
    ///     Creates a new instance of a transaction history dialog initialized with randomly generated
    ///     data.
    /// </summary>
    /// <remarks>
    ///     Internally this calls <see cref="CreateDefaultInstance" /> with a randomly generated viewer.
    /// </remarks>
    public static TransactionHistoryDialog CreateDebugInstance() => CreateDefaultInstance(PseudoDataGenerator.GeneratePseudoViewerData(1, 30)[0]);
}
