#if DEBUG

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
using RimWorld;
using StreamKit.UX;
using StreamKit.UX.Extensions;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.Windows;

internal sealed partial class SettingsWindow
{
    private static readonly DebugWindow[] DebugWindows =
    [
        new(KitTranslations.OpenLedgerWindowDebugAction, LedgerWindow.CreateDebugInstance),
        new(KitTranslations.OpenTransactionHistoryWindowDebugAction, TransactionHistoryDialog.CreateDebugInstance),
        new(KitTranslations.OpenRuntimeFlagsWindowDebugAction, RuntimeFlagWindow.CreateInstance),
        new(KitTranslations.OpenPlatformsWindowDebugAction, PlatformsWindow.CreateDebugInstance)
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

        var windowColumn = new Rect(0f, 0f, Mathf.Floor(region.width * 0.3f), region.height);

        GUI.BeginGroup(windowColumn);
        DrawDebugWindowColumn(windowColumn.AtZero());
        GUI.EndGroup();

        GUI.EndGroup();
    }

    private sealed record DebugWindow(string Label, Func<Window> WindowFunc);
}

#endif
