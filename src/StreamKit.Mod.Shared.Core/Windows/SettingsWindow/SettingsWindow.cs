using System;
using System.Collections.Generic;
using StreamKit.Mod.Api;
using StreamKit.Mod.Shared.Core.Settings;
using StreamKit.Mod.Shared.Extensions;
using StreamKit.Mod.Shared.UX;
using UnityEngine;

namespace StreamKit.Mod.Shared.Core.Windows;

// TODO: Currently point decay settings aren't drawn on screen.

public sealed partial class SettingsWindow : ProxySettingsWindow
{
    private readonly CommandSettings _commandSettingsInstance = new();
    private readonly MoralitySettings _moralitySettingsInstance = new();
    private readonly PawnSettings _pawnSettingsInstance = new();
    private readonly PointSettings _pointSettingsInstance = new();
    private readonly PollSettings _pollSettingsInstance = new();
    private readonly StoreSettings _storeSettingsInstance = new();
    private readonly TabularDrawer _tabWorker;
    private readonly TwitchSettings _twitchSettingsInstance = new();
    private Vector2 _commandScrollPosition = Vector2.zero;
    private Vector2 _moralityScrollPosition = Vector2.zero;
    private Vector2 _pawnScrollPosition = Vector2.zero;
    private Vector2 _pointScrollPosition = Vector2.zero;
    private Vector2 _pollScrollPosition = Vector2.zero;
    private Vector2 _storeScrollPosition = Vector2.zero;
    private Vector2 _twitchScrollPosition = Vector2.zero;

    /// <inheritdoc />
    public SettingsWindow(Verse.Mod mod) : base(mod)
    {
        doCloseX = false;

        _tabWorker = new TabularDrawer.Builder().WithTab(
                o =>
                {
                    IReadOnlyList<ModSettingDrawer> commandSettings = ModSettingFactory.FromInstance(_commandSettingsInstance);

                    o.Icon = Icons.Message;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Commands".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's command system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, commandSettings, ref _commandScrollPosition);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSettingDrawer> moralitySettings = ModSettingFactory.FromInstance(_moralitySettingsInstance);

                    o.Icon = Icons.ScaleBalanced;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Morality".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's morality system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, moralitySettings, ref _moralityScrollPosition);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSettingDrawer> pawnSettings = ModSettingFactory.FromInstance(_pawnSettingsInstance);

                    o.Icon = Icons.People;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Pawns".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's pawn system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, pawnSettings, ref _pawnScrollPosition);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSettingDrawer> pointSettings = ModSettingFactory.FromInstance(_pointSettingsInstance);

                    o.Icon = Icons.PiggyBank;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Points".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's point system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, pointSettings, ref _pointScrollPosition);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSettingDrawer> pollSettings = ModSettingFactory.FromInstance(_pollSettingsInstance);

                    o.Icon = Icons.SquarePollVertical;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Poll".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's polling system functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, pollSettings, ref _pollScrollPosition);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSettingDrawer> storeSettings = ModSettingFactory.FromInstance(_storeSettingsInstance);

                    o.Icon = Icons.Store;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Store".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's store functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, storeSettings, ref _storeScrollPosition);
                }
            )
           .WithTab(
                o =>
                {
                    IReadOnlyList<ModSettingDrawer> twitchSettings = ModSettingFactory.FromInstance(_twitchSettingsInstance);

                    o.Icon = Icons.Twitch;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Twitch".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affect how the mod's Twitch connection functions.".MarkNotTranslated();
                    o.Drawer = region => DrawTabSettings(region, twitchSettings, ref _twitchScrollPosition);
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

    private static void DrawSetting(Rect region, ModSettingDrawer setting)
    {
        (Rect labelRegion, Rect fieldRegion) = region.Split();

        LabelDrawer.Draw(labelRegion, setting.Label);
        setting.Drawer.Draw(ref fieldRegion);
    }
}
