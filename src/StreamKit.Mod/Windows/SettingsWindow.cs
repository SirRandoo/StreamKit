// MIT License
// 
// Copyright (c) 2022 SirRandoo
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
using SirRandoo.CommonLib.Windows;
using SirRandoo.CommonLib.Workers;
using UnityEngine;
using Verse;

namespace StreamKit.Mod;

public partial class SettingsWindow : ProxySettingsWindow
{
    private readonly TabWorker _tabWorker = new MaterializedTabWorker();

    /// <inheritdoc/>
    public SettingsWindow(Verse.Mod mod) : base(mod)
    {
    }

    /// <inheritdoc/>
    protected override void GetTranslations()
    {
        base.GetTranslations();

    #if DEBUG
        _tabWorker.AddTab("tabs.debug", "Debug", "A collection of debugging utilities for StreamKit", DrawDebugTab);
    #endif
    }

    /// <inheritdoc/>
    public override void DoWindowContents(Rect inRect)
    {
        GUI.BeginGroup(inRect);

        GUI.EndGroup();
    }

    private static void DrawDebugTab(Rect region)
    {
        GUI.BeginGroup(region);

        var windowColumn = new Rect(0f, 0f, (float)Math.Floor(region.width * 0.333f), region.height);

        GUI.BeginGroup(windowColumn);
        DrawDebugWindowColumn(windowColumn.AtZero());
        GUI.EndGroup();

        GUI.EndGroup();
    }
}
