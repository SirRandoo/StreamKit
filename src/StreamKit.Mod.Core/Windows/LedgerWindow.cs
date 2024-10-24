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
using System.Collections.ObjectModel;
using System.Linq;
using HarmonyLib;
using RimWorld;
using StreamKit.Mod.Api;
using StreamKit.Mod.Core.Dialogs;
using StreamKit.Mod.Shared.Logging;
using StreamKit.Shared.Interfaces;
using StreamKit.UX;
using StreamKit.UX.Drawers;
using StreamKit.UX.Extensions;
using UnityEngine;
using Verse;
using DropdownDrawer = StreamKit.Mod.Core.UX.DropdownDrawer;
using IIdentifiable = StreamKit.Shared.Interfaces.IIdentifiable;
using Logger = NLog.Logger;

namespace StreamKit.Mod.Core.Windows;

// TODO: The karma and balance fields don't support thousands separators.
// TODO: The karma and balance fields don't support freeform input.
// TODO: The karma and balance fields don't indicate when a value isn't a valid number.
// TODO: The karma and balance fields don't properly clamp their values to ranges.

public class LedgerWindow : Window
{
    private const float RowSplitPercentage = 0.35f;
    private static readonly Logger Logger = KitLogManager.GetLogger("StreamKit.Windows.Ledger");
    private readonly QuickSearchWidget _searchWidget = new();

    private IReadOnlyList<ILedger> _dropdownItems = null!; // TODO: Ensure this points to all ledgers when ledger registry is implemented.

    private bool _isDebugInstance;
    private ILedger? _ledger;
    private Vector2 _listScrollPosition = Vector2.zero;
    private IUser? _viewer;
    private string? _viewerBalanceBuffer;
    private bool _viewerBalanceBufferValid;
    private string? _viewerKarmaBuffer;
    private bool _viewerKarmaBufferValid;
    private IReadOnlyList<IUser> _workingList = new List<IUser>(0);

    private LedgerWindow()
    {
    }

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

    // TODO: Replace the right branch with the actual viewer list.
    protected ILedger? ViewerList => _isDebugInstance ? _ledger : null;

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        var ledgerListRegion = new Rect(0f, 0f, inRect.width * 0.4f, inRect.height);
        var headerRegion = new Rect(ledgerListRegion.width + 10f, 0f, inRect.width - ledgerListRegion.width - 10f, UiConstants.LineHeight);
        var contentRegion = new Rect(headerRegion.x, headerRegion.height + 10f, headerRegion.width, ledgerListRegion.height - headerRegion.height - 10f);

        GUI.BeginGroup(inRect);

        GUI.BeginGroup(headerRegion);
        DrawHeader(GenUI.AtZero(headerRegion));
        GUI.EndGroup();

        GUI.BeginGroup(ledgerListRegion);
        DrawLedgerOverview(GenUI.AtZero(ledgerListRegion));
        GUI.EndGroup();

        GUI.BeginGroup(contentRegion);

        if (_viewer != null)
        {
            DrawViewerPanel(GenUI.AtZero(contentRegion));
        }

        GUI.EndGroup();

        GUI.EndGroup();
    }

    private void DrawLedgerOverview(Rect region)
    {
        var searchRegion = new Rect(0f, 0f, region.width, UiConstants.LineHeight);
        var listRegion = new Rect(0f, UiConstants.LineHeight + 10f, region.width, region.height - UiConstants.LineHeight - 10f);
        Rect innerListRegion = GenUI.ContractedBy(listRegion, 5f);

        GUI.BeginGroup(searchRegion);
        _searchWidget.OnGUI(GenUI.AtZero(searchRegion), FilterViewerList);
        GUI.EndGroup();

        Widgets.DrawMenuSection(listRegion);

        GUI.BeginGroup(innerListRegion);
        DrawViewerList(GenUI.AtZero(innerListRegion));
        GUI.EndGroup();
    }

    private void DrawViewerList(Rect region)
    {
        int totalViewers = _workingList.Count;
        float scrollViewHeight = UiConstants.LineHeight * totalViewers;
        var scrollView = new Rect(0f, 0f, region.width - (scrollViewHeight > region.height ? 16f : 0f), scrollViewHeight);

        GUI.BeginGroup(region);
        _listScrollPosition = GUI.BeginScrollView(region, _listScrollPosition, scrollView);

        var workingListDirty = false;

        for (var index = 0; index < totalViewers; index++)
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

            IUser? viewer = _workingList[index];

            if (_viewer == viewer)
            {
                Widgets.DrawHighlightSelected(lineRegion);
            }

            // TODO: Draw viewer's platform icon when platforms are implemented.
            LabelDrawer.DrawLabel(lineRegion, viewer.Name);

            if (ButtonDrawer.DrawFieldButton(lineRegion, TexButton.Delete, KitTranslations.DeleteViewerTooltip))
            {
                _ledger!.Data.Unregister(viewer);

                workingListDirty = true;
            }

            if (Widgets.ButtonInvisible(lineRegion))
            {
                _viewer = viewer;

                _viewerBalanceBuffer = viewer.Points.ToString();
                _viewerBalanceBufferValid = true;

                _viewerKarmaBuffer = viewer.Karma.ToString();
                _viewerKarmaBufferValid = true;

                Find.WindowStack.TryRemove(typeof(TransactionHistoryDialog), false);
            }

            Widgets.DrawHighlightIfMouseover(lineRegion);
        }

        GUI.EndScrollView();
        GUI.EndGroup();

        if (workingListDirty)
        {
            FilterViewerList();
        }
    }

    private void DrawHeader(Rect region)
    {
        (Rect ledgerLabelRegion, Rect ledgerDropdownRegion) = region.Split(RowSplitPercentage);

        LabelDrawer.DrawLabel(ledgerLabelRegion, KitTranslations.CurrentLedger, TextAnchor.MiddleRight);
        DropdownDrawer.Draw(ledgerDropdownRegion, _ledger!, _dropdownItems, UpdateLedger);
    }

    private void DrawViewerPanel(Rect region)
    {
        var lineRegion = new Rect(0f, 0f, region.width, UiConstants.LineHeight);

        DrawUsernameField(lineRegion);

        lineRegion = RectExtensions.Shift(lineRegion, Direction8Way.South);
        DrawBalanceField(lineRegion);

        lineRegion = RectExtensions.Shift(lineRegion, Direction8Way.South);
        DrawKarmaField(lineRegion);


        lineRegion = RectExtensions.Shift(lineRegion, Direction8Way.South);
        (Rect _, Rect transactionsBtnRegion) = lineRegion.Split(RowSplitPercentage);

        if (Widgets.ButtonText(transactionsBtnRegion, KitTranslations.ViewTransactionsText, overrideTextAnchor: TextAnchor.MiddleCenter))
        {
            Find.WindowStack.Add(TransactionHistoryDialog.CreateDefaultInstance(_viewer!));
        }

        var lastSeenRegion = new Rect(0f, region.height - Text.SmallFontHeight, region.width, Text.SmallFontHeight);
        LabelDrawer.DrawLabel(lastSeenRegion, string.Format(KitTranslations.LedgerLastSeen, FormatLastSeen(_viewer!)));
    }

    private void DrawKarmaField(Rect region)
    {
        (Rect labelRegion, Rect fieldRegion) = region.Split(RowSplitPercentage);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.CommonTextKarma);

        var addBtnRegion = new Rect(fieldRegion.x, fieldRegion.y, fieldRegion.height, fieldRegion.height);
        var rmvBtnRegion = new Rect(fieldRegion.x + fieldRegion.width - fieldRegion.height, fieldRegion.y, fieldRegion.height, fieldRegion.height);
        var numberFieldRegion = new Rect(fieldRegion.x + fieldRegion.height + 5f, fieldRegion.y, fieldRegion.width - fieldRegion.height * 2f - 10f, fieldRegion.height);

        if (FieldDrawer.DrawNumberField(numberFieldRegion, out int newKarma, ref _viewerKarmaBuffer!, ref _viewerKarmaBufferValid))
        {
            _viewer!.Karma = (short)newKarma;
        }


        Text.Font = GameFont.Medium;

        if (Widgets.ButtonText(addBtnRegion, "+", overrideTextAnchor: TextAnchor.MiddleCenter))
        {
            var dialog = new ShortEntryDialog(maximum: (short)(ModKit.Instance.Settings.Morality.KarmaRange.Minimum - _viewer!.Karma));
            dialog.NumberEntered += (_, e) => _viewer.Karma += e;

            Find.WindowStack.Add(dialog);
        }

        TooltipHandler.TipRegion(addBtnRegion, KitTranslations.AddViewerKarmaTooltip);

        if (Widgets.ButtonText(rmvBtnRegion, "-", overrideTextAnchor: TextAnchor.MiddleCenter))
        {
            var dialog = new ShortEntryDialog((short)(ModKit.Instance.Settings.Morality.KarmaRange.Maximum + _viewer!.Karma), 0);
            dialog.NumberEntered += (_, e) => _viewer.Karma -= e;

            Find.WindowStack.Add(dialog);
        }

        TooltipHandler.TipRegion(rmvBtnRegion, KitTranslations.RemoveViewerKarmaTooltip);

        Text.Font = GameFont.Small;
    }

    private void DrawBalanceField(Rect region)
    {
        (Rect labelRegion, Rect fieldRegion) = region.Split(RowSplitPercentage);

        LabelDrawer.DrawLabel(labelRegion, KitTranslations.CommonTextBalance);


        var addBtnRegion = new Rect(fieldRegion.x, fieldRegion.y, fieldRegion.height, fieldRegion.height);
        var rmvBtnRegion = new Rect(fieldRegion.x + fieldRegion.width - fieldRegion.height, fieldRegion.y, fieldRegion.height, fieldRegion.height);
        var numberFieldRegion = new Rect(fieldRegion.x + fieldRegion.height + 5f, fieldRegion.y, fieldRegion.width - fieldRegion.height * 2f - 10f, fieldRegion.height);

        if (FieldDrawer.DrawNumberField(numberFieldRegion, out int newBalance, ref _viewerBalanceBuffer!, ref _viewerBalanceBufferValid))
        {
            _viewer!.Points = newBalance;
        }


        Text.Font = GameFont.Medium;

        if (Widgets.ButtonText(addBtnRegion, "+", overrideTextAnchor: TextAnchor.MiddleCenter))
        {
            var dialog = new LongEntryDialog(maximum: int.MaxValue - _viewer!.Points);
            dialog.NumberEntered += (_, e) => _viewer.Points += e;

            Find.WindowStack.Add(dialog);
        }

        TooltipHandler.TipRegion(addBtnRegion, KitTranslations.AddViewerPointsTooltip);

        if (Widgets.ButtonText(rmvBtnRegion, "-", overrideTextAnchor: TextAnchor.MiddleCenter))
        {
            var dialog = new LongEntryDialog(int.MinValue + _viewer!.Points, 0);
            dialog.NumberEntered += (_, e) => _viewer.Points -= e;

            Find.WindowStack.Add(dialog);
        }

        TooltipHandler.TipRegion(rmvBtnRegion, KitTranslations.RemoveViewerPointsTooltip);

        Text.Font = GameFont.Small;
    }

    private void DrawUsernameField(Rect region)
    {
        (Rect nameLabelRegion, Rect nameFieldRegion) = region.Split(RowSplitPercentage);

        LabelDrawer.DrawLabel(nameLabelRegion, KitTranslations.CommonTextUsername);

        if (FieldDrawer.DrawTextField(nameFieldRegion, _viewer!.Name, out string? newName))
        {
            _viewer.Name = newName;
        }
    }

    private void FilterViewerList()
    {
        if (string.IsNullOrEmpty(_searchWidget.filter.Text))
        {
            _workingList = ViewerList!.Data.AllRegistrants;
        }

        var copy = new List<IUser>();
        IReadOnlyList<IUser> registrants = ViewerList!.Data.AllRegistrants;


        for (var index = 0; index < registrants.Count; index++)
        {
            IUser? viewer = registrants[index];

            if (string.Compare(viewer.Name, _searchWidget.filter.Text, StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                copy.Add(viewer);
            }
            else if (viewer == _viewer)
            {
                _viewer = null;
            }
        }

        _workingList = copy.AsReadOnly();
    }

    private void UpdateLedger(IIdentifiable newLedger)
    {
        _ledger = (ILedger)newLedger;

        FilterViewerList();
    }

    private static string FormatLastSeen(IUser viewer)
    {
        DateTime currentTime = DateTime.UtcNow;
        TimeSpan offset = currentTime - viewer.LastSeen;

        double totalDays = offset.TotalDays;

        if (totalDays > 0)
        {
            return string.Format(KitTranslations.LedgerLastSeenDays, totalDays.ToString("F2"));
        }

        double totalHours = offset.TotalHours;

        if (totalHours > 0)
        {
            return string.Format(KitTranslations.LedgerLastSeenHours, totalHours.ToString("F2"));
        }

        double totalMinutes = offset.TotalMinutes;

        if (totalMinutes > 0)
        {
            return string.Format(KitTranslations.LedgerLastSeenMinutes, totalMinutes.ToString("F2"));
        }

        return string.Format(KitTranslations.LedgerLastSeenSeconds, offset.TotalSeconds.ToString("F2"));
    }

    /// <inheritdoc />
    public override void PreOpen()
    {
        base.PreOpen();

        if (ViewerList is not null)
        {
            _workingList = ViewerList.Data.AllRegistrants;

            return;
        }

        Logger.Warn("Closing window as no ledger was provided to the ledger window. If this happens repeatedly, you should ask for help from the developer(s).");

        Close(false);
    }

#if DEBUG
    /// <summary>
    ///     Creates a new ledger window seeded with debug data.
    /// </summary>
    public static LedgerWindow CreateDebugInstance()
    {
        ILedger ledger = PseudoDataGenerator.Instance.GeneratePseudoLedger(100, 50);

        return new LedgerWindow
        {
            _isDebugInstance = true,
            _ledger = ledger,
            _dropdownItems = Enumerable.Range(0, 9).Select(_ => PseudoDataGenerator.Instance.GeneratePseudoLedger(20, 10)).AddItem(ledger).ToList()
        };
    }
#endif

    /// <summary>
    ///     Creates an empty ledger window without any data seeded.
    /// </summary>
    /// <returns></returns>
    public static LedgerWindow CreateDefaultInstance() => new();
}
