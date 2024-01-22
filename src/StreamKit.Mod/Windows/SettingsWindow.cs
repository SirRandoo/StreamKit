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
using System.Collections.Generic;
using SirRandoo.CommonLib.Enums;
using SirRandoo.CommonLib.Helpers;
using SirRandoo.CommonLib.Windows;
using StreamKit.Api;
using StreamKit.Api.UX;
using StreamKit.Mod.Settings;
using UnityEngine;
using Verse;

namespace StreamKit.Mod;

// TODO: Currently point decay settings aren't drawn on screen.

public partial class SettingsWindow : ProxySettingsWindow
{
    private readonly CommandSettings _commandSettingsInstance = new();
    private readonly MoralitySettings _moralitySettingsInstance = new();
    private readonly PawnSettings _pawnSettingsInstance = new();
    private readonly PointSettings _pointSettingsInstance = new();
    private readonly PollSettings _pollSettingsInstance = new();
    private readonly StoreSettings _storeSettingsInstance = new();
    private readonly TabularDrawer _tabWorker;
    private readonly TwitchSettings _twitchSettingsInstance = new();
    private Vector2 _commandScrollPos = Vector2.zero;
    private Vector2 _moralityScrollPos = Vector2.zero;
    private Vector2 _pawnScrollPos = Vector2.zero;
    private Vector2 _pointScrollPos = Vector2.zero;
    private Vector2 _pollScrollPos = Vector2.zero;
    private Vector2 _storeScrollPos = Vector2.zero;
    private Vector2 _twitchScrollPos = Vector2.zero;

    /// <inheritdoc />
    public SettingsWindow(Verse.Mod mod) : base(mod)
    {
        doCloseX = false;

        _tabWorker = new TabularDrawer.Builder().WithTab(
                o =>
                {
                    IReadOnlyList<ModSetting> commandSettings = ModSettingFactory.FromInstance(_commandSettingsInstance);

                    o.Icon = Icons.Message;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Commands".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's command system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, commandSettings, ref _commandScrollPos);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSetting> moralitySettings = ModSettingFactory.FromInstance(_moralitySettingsInstance);

                    o.Icon = Icons.ScaleBalanced;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Morality".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's morality system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, moralitySettings, ref _moralityScrollPos);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSetting> pawnSettings = ModSettingFactory.FromInstance(_pawnSettingsInstance);

                    o.Icon = Icons.People;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Pawns".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's pawn system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, pawnSettings, ref _pawnScrollPos);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSetting> pointSettings = ModSettingFactory.FromInstance(_pointSettingsInstance);

                    o.Icon = Icons.PiggyBank;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Points".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's point system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, pointSettings, ref _pointScrollPos);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSetting> pollSettings = ModSettingFactory.FromInstance(_pollSettingsInstance);

                    o.Icon = Icons.SquarePollVertical;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Poll".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's polling system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, pollSettings, ref _pollScrollPos);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSetting> storeSettings = ModSettingFactory.FromInstance(_storeSettingsInstance);

                    o.Icon = Icons.Store;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Store".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's store functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, storeSettings, ref _storeScrollPos);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSetting> twitchSettings = ModSettingFactory.FromInstance(_twitchSettingsInstance);

                    o.Icon = Icons.Twitch;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Twitch".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's Twitch connection functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, twitchSettings, ref _twitchScrollPos);
                }
            )
        #if DEBUG
           .WithTab(
                o =>
                {
                    o.Icon = Icons.Bug;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Debug".MarkNotTranslated();
                    o.Tooltip = "A collection of interesting content for debugging the mod.".MarkNotTranslated();
                    o.Drawer = DrawDebugTab;
                }
            )
        #endif
           .Build();
    }

    /// <inheritdoc />
    protected override float Margin => 0f;

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        GUI.BeginGroup(inRect);

        _tabWorker.Draw(inRect.AtZero());

        GUI.EndGroup();
    }

    private static void DrawSetting(Rect region, ModSetting setting)
    {
        (Rect labelRegion, Rect fieldRegion) = region.Split();

        UiHelper.Label(labelRegion, setting.Label, TextAnchor.MiddleLeft);
        setting.Drawer.Draw(ref fieldRegion);
    }
}
