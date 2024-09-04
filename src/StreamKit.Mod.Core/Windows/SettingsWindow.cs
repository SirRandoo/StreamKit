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

using SirRandoo.UX;
using SirRandoo.UX.Drawers;
using SirRandoo.UX.Extensions;
using StreamKit.Mod.Core.Pages;
using UnityEngine;

namespace StreamKit.Mod.Core.Windows;

// TODO: Currently point decay settings aren't drawn on screen.

internal sealed partial class SettingsWindow : ProxySettingsWindow
{
    private readonly ClientSettingsPage _clientSettingsPage = new();
    private readonly CommandSettingsPage _commandSettingsPage = new();
    private readonly MoralitySettingsPage _moralitySettingsPage = new();
    private readonly PawnSettingsPage _pawnSettingsPage = new();
    private readonly PointSettingsPage _pointSettingsPage = PointSettingsPage.CreateInstance();
    private readonly PollSettingsPage _pollSettingsPage = new();
    private readonly StoreSettingsPage _storeSettingsPage = new();
    private readonly TabularDrawer _tabWorker;

    /// <inheritdoc />
    public SettingsWindow(Verse.Mod mod) : base(mod)
    {
        doCloseX = false;

        _tabWorker = new TabularDrawer.Builder().WithTab(
                "client",
                o =>
                {
                    o.Icon = Icons.Person.Value;
                    o.Layout = IconLayout.IconAndText;
                    o.Drawer = _clientSettingsPage.Draw;
                    o.Label = KitTranslations.SettingsTabClient;
                    o.Tooltip = KitTranslations.SettingsTabClientTooltip;
                }
            )
           .WithTab(
                "commands",
                options =>
                {
                    options.Icon = Icons.Message.Value;
                    options.Layout = IconLayout.IconAndText;
                    options.Drawer = _commandSettingsPage.Draw;
                    options.Label = KitTranslations.SettingsTabCommands;
                    options.Tooltip = KitTranslations.SettingsTabCommandTooltip;
                }
            )
           .WithTab(
                "morality",
                options =>
                {
                    options.Icon = Icons.ScaleBalanced.Value;
                    options.Layout = IconLayout.IconAndText;
                    options.Drawer = _moralitySettingsPage.Draw;
                    options.Label = KitTranslations.SettingsTabMorality;
                    options.Tooltip = KitTranslations.SettingsTabMoralityTooltip;
                }
            )
           .WithTab(
                "pawns",
                options =>
                {
                    options.Icon = Icons.PeopleGroup.Value;
                    options.Layout = IconLayout.IconAndText;
                    options.Drawer = _pawnSettingsPage.Draw;
                    options.Label = KitTranslations.SettingsTabPawns;
                    options.Tooltip = KitTranslations.SettingsTabPawnsTooltip;
                }
            )
           .WithTab(
                "points",
                options =>
                {
                    options.Icon = Icons.PiggyBank.Value;
                    options.Layout = IconLayout.IconAndText;
                    options.Drawer = _pointSettingsPage.Draw;
                    options.Label = KitTranslations.SettingsTabPoints;
                    options.Tooltip = KitTranslations.SettingsTabPointsTooltip;
                }
            )
           .WithTab(
                "poll",
                options =>
                {
                    options.Icon = Icons.SquarePollVertical.Value;
                    options.Layout = IconLayout.IconAndText;
                    options.Drawer = _pollSettingsPage.Draw;
                    options.Label = KitTranslations.SettingsTabPoll;
                    options.Tooltip = KitTranslations.SettingsTabPollTooltip;
                }
            )
           .WithTab(
                "store",
                options =>
                {
                    options.Icon = Icons.Store.Value;
                    options.Layout = IconLayout.IconAndText;
                    options.Drawer = _storeSettingsPage.Draw;
                    options.Label = KitTranslations.SettingsTabStore;
                    options.Tooltip = KitTranslations.SettingsTabStoreTooltip;
                }
            )
#if DEBUG
           .WithTab(
                "debug",
                o =>
                {
                    o.Drawer = DrawDebugTab;
                    o.Icon = Icons.Bug.Value;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = KitTranslations.SettingsTabDebug;
                    o.Tooltip = KitTranslations.SettingsTabDebugTooltip;
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
}
