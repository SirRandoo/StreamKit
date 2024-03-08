using System;
using System.Collections.Generic;
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
        new DebugWindow("Open runtime flag window".MarkNotTranslated(), RuntimeFlagWindow.CreateInstance)
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

    private static void DrawTabSettings(Rect region, IReadOnlyList<ModSettingDrawer> settings, ref Vector2 scrollPosition)
    {
        float height = settings.Count * UiConstants.LineHeight * 2f;
        var viewport = new Rect(0f, 0f, region.width - (height > region.height ? 16f : 0f), height);

        GUI.BeginGroup(region);
        scrollPosition = GUI.BeginScrollView(region, scrollPosition, viewport);

        var yPosition = 0f;

        for (var i = 0; i < settings.Count; i++)
        {
            var lineRegion = new Rect(0f, yPosition, viewport.width, UiConstants.LineHeight);
            yPosition += lineRegion.height;

            if (!lineRegion.IsVisible(viewport, scrollPosition))
            {
                continue;
            }

            ModSettingDrawer? setting = settings[i];
            DrawSetting(lineRegion, setting);

            if (Mouse.IsOver(lineRegion))
            {
                Widgets.DrawLightHighlight(lineRegion);
            }

            if (Widgets.ButtonInvisible(lineRegion))
            {
                setting.Drawer.Toggle();
            }

            if (string.IsNullOrEmpty(setting.Description))
            {
                continue;
            }

            Vector2 descriptionSize = DescriptionDrawer.GetTextBlockSize(setting.Description!, lineRegion.width, 0.8f);
            var descriptionRegion = new Rect(0f, yPosition, descriptionSize.x, descriptionSize.y);
            yPosition += descriptionRegion.height;

            if (!descriptionRegion.IsVisible(viewport, scrollPosition))
            {
                continue;
            }

            DescriptionDrawer.DrawDescription(descriptionRegion, setting.Description!);
        }

        GUI.EndScrollView();
        GUI.EndGroup();
    }

    private sealed record DebugWindow(string Label, Func<Window> WindowFunc);
}
