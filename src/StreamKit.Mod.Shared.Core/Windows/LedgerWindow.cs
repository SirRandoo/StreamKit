using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HarmonyLib;
using RimWorld;
using StreamKit.Common.Data.Abstractions;
using StreamKit.Mod.Api;
using StreamKit.Mod.Shared.Extensions;
using StreamKit.Mod.Shared.UX;
using UnityEngine;
using Verse;
using Logger = NLog.Logger;

namespace StreamKit.Mod.Shared.Core.Windows;

// TODO: The karma and balance fields don't support thousands separators.
// TODO: The karma and balance fields don't support freeform input.
// TODO: The karma and balance fields don't indicate when a value isn't a valid number.
// TODO: The karma and balance fields don't properly clamp their values to ranges.

public class LedgerWindow : Window
{
    private const float RowSplitPercentage = 0.35f;
    private static readonly Logger Logger = LogManager.Factory.GetCurrentClassLogger();
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
    private IList<IUser> _workingList = ImmutableList<IUser>.Empty;

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
            LabelDrawer.Draw(lineRegion, viewer.Name);

            #if RW_14
                Texture2D deleteBtnTexture = TexButton.DeleteX;
            #else
                Texture2D deleteBtnTexture = TexButton.Delete;
            #endif

            if (ButtonDrawer.DrawFieldButton(lineRegion, deleteBtnTexture, "Deletes the viewer's information from the ledger.".MarkNotTranslated()))
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

        LabelDrawer.Draw(ledgerLabelRegion, "Current ledger:".MarkNotTranslated(), TextAnchor.MiddleRight);
        DropdownDrawer.Draw(ledgerDropdownRegion, _ledger!, _dropdownItems, UpdateLedger);
    }

    private void DrawViewerPanel(Rect region)
    {
        var lineRegion = new Rect(0f, 0f, region.width, UiConstants.LineHeight);

        DrawUsernameField(lineRegion);

        lineRegion = lineRegion.Shift(Direction8Way.South);
        DrawBalanceField(lineRegion);

        lineRegion = lineRegion.Shift(Direction8Way.South);
        DrawKarmaField(lineRegion);


        lineRegion = lineRegion.Shift(Direction8Way.South);
        (Rect _, Rect transactionsBtnRegion) = lineRegion.Split(RowSplitPercentage);

        if (Widgets.ButtonText(transactionsBtnRegion, "View transactions".MarkNotTranslated(), overrideTextAnchor: TextAnchor.MiddleCenter))
        {
            Find.WindowStack.Add(TransactionHistoryDialog.CreateDefaultInstance(_viewer!));
        }

        var lastSeenRegion = new Rect(0f, region.height - Text.SmallFontHeight, region.width, Text.SmallFontHeight);
        LabelDrawer.Draw(lastSeenRegion, $"Last seen: {FormatLastSeen(_viewer!)}".MarkNotTranslated());
    }

    private void DrawKarmaField(Rect region)
    {
        (Rect labelRegion, Rect fieldRegion) = region.Split(RowSplitPercentage);

        LabelDrawer.Draw(labelRegion, "Karma".MarkNotTranslated());

        var addBtnRegion = new Rect(fieldRegion.x, fieldRegion.y, fieldRegion.height, fieldRegion.height);
        var rmvBtnRegion = new Rect(fieldRegion.x + fieldRegion.width - fieldRegion.height, fieldRegion.y, fieldRegion.height, fieldRegion.height);
        var numberFieldRegion = new Rect(fieldRegion.x + fieldRegion.height + 5f, fieldRegion.y, fieldRegion.width - fieldRegion.height * 2f - 10f, fieldRegion.height);

        if (FieldDrawer.DrawNumberField(numberFieldRegion, out int newKarma, ref _viewerKarmaBuffer!, ref _viewerKarmaBufferValid))
        {
            _viewer!.Karma = (short)newKarma;
        }


        Text.Font = GameFont.Medium;

        if (Widgets.ButtonText(addBtnRegion, "+", overrideTextAnchor: TextAnchor.MiddleCenter))

            // TODO: Revise the "maximum karma" formula when mod settings are implement.
        {
            Find.WindowStack.Add(new ShortEntryDialog(i => _viewer!.Karma += i, maximum: (short)(short.MaxValue - _viewer!.Karma)));
        }

        TooltipHandler.TipRegion(addBtnRegion, "Click to add karma to the viewer's karma.".MarkNotTranslated());

        if (Widgets.ButtonText(rmvBtnRegion, "-", overrideTextAnchor: TextAnchor.MiddleCenter))

            // TODO: Revise the "maximum karma" formula when mod settings are implement.
        {
            Find.WindowStack.Add(new ShortEntryDialog(i => _viewer!.Karma -= i, (short)(short.MinValue + _viewer!.Karma), 0));
        }

        TooltipHandler.TipRegion(rmvBtnRegion, "Click to remove karma from a viewer's karma.".MarkNotTranslated());

        Text.Font = GameFont.Small;
    }

    private void DrawBalanceField(Rect region)
    {
        (Rect labelRegion, Rect fieldRegion) = region.Split(RowSplitPercentage);

        LabelDrawer.Draw(labelRegion, "Balance".MarkNotTranslated());


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
            Find.WindowStack.Add(new LongEntryDialog(f => _viewer!.Points += f, maximum: int.MaxValue - _viewer!.Points));
        }

        TooltipHandler.TipRegion(addBtnRegion, "Click to add points to a viewer's balance.".MarkNotTranslated());

        if (Widgets.ButtonText(rmvBtnRegion, "-", overrideTextAnchor: TextAnchor.MiddleCenter))
        {
            Find.WindowStack.Add(new LongEntryDialog(f => _viewer!.Points -= f, int.MinValue + _viewer!.Points, 0));
        }

        TooltipHandler.TipRegion(rmvBtnRegion, "Click to remove points from a viewer's balance.".MarkNotTranslated());

        Text.Font = GameFont.Small;
    }

    private void DrawUsernameField(Rect region)
    {
        (Rect nameLabelRegion, Rect nameFieldRegion) = region.Split(RowSplitPercentage);

        LabelDrawer.Draw(nameLabelRegion, "Username".MarkNotTranslated());

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
        IList<IUser> registrants = ViewerList!.Data.AllRegistrants;


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

        _workingList = copy.ToImmutableList();
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
            return $"{totalDays:F2} days ago.".MarkNotTranslated();
        }

        double totalHours = offset.TotalHours;

        if (totalHours > 0)
        {
            return $"{totalHours:F2} hours ago.".MarkNotTranslated();
        }

        double totalMinutes = offset.TotalMinutes;

        if (totalMinutes > 0)
        {
            return $"{totalMinutes:F2} minutes ago.".MarkNotTranslated();
        }

        return $"{offset.TotalSeconds:F2} seconds ago.".MarkNotTranslated();
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
            _dropdownItems = Enumerable.Range(0, 9).Select(_ => PseudoDataGenerator.Instance.GeneratePseudoLedger(20, 10)).AddItem(ledger).ToImmutableList()
        };
    }
#endif

    /// <summary>
    ///     Creates an empty ledger window without any data seeded.
    /// </summary>
    /// <returns></returns>
    public static LedgerWindow CreateDefaultInstance() => new();
}
