#if DEBUG

using System;
using RimWorld;
using StreamKit.Mod.Api;
using StreamKit.Mod.Shared.UX;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Shared.Core.Windows;

public sealed partial class SettingsWindow
{
    private static readonly DebugWindow[] DebugWindows =
    [
        new DebugWindow("Open ledger window".MarkNotTranslated(), LedgerWindow.CreateDebugInstance),
        new DebugWindow("Open transaction history window".MarkNotTranslated(), TransactionHistoryDialog.CreateDebugInstance),
        new DebugWindow("Open runtime flag window".MarkNotTranslated(), RuntimeFlagWindow.CreateInstance),
        new DebugWindow("Open platforms window".MarkNotTranslated(), PlatformsWindow.CreateDebugInstance)
    ];

    private static void DrawDebugWindowColumn(Rect region)
    {
        GUI.BeginGroup(region);

        var buttonRegion = new Rect(0f, 0f, region.width, UiConstants.LineHeight);

        for (var i = 0; i < DebugWindows.Length; i++)
        {
            if (i > 0)
            {
                buttonRegion = buttonRegion.Shift(Direction8Way.South, 0f);
            }

            if (Widgets.ButtonText(buttonRegion, DebugWindows[i].Label))
            {
                Find.WindowStack.Add(DebugWindows[i].WindowFunc());
            }
        }

        GUI.EndGroup();
    }

    private static void DrawDebugTab(Rect region)
    {
        GUI.BeginGroup(region);

        var windowColumn = new Rect(0f, 0f, (float)Math.Floor(region.width * 0.3f), region.height);

        GUI.BeginGroup(windowColumn);
        DrawDebugWindowColumn(windowColumn.AtZero());
        GUI.EndGroup();

        GUI.EndGroup();
    }

    private sealed record DebugWindow(string Label, Func<Window> WindowFunc);
}

#endif
