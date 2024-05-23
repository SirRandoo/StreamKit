using System;
using System.Collections.Generic;
using RimWorld;
using StreamKit.Common.Data.Abstractions;
using StreamKit.Mod.Api;
using StreamKit.Mod.Shared.Core.Settings;
using StreamKit.Mod.Shared.Extensions;
using StreamKit.Mod.Shared.UX;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Shared.Core.Windows;

// TODO: Currently point decay settings aren't drawn on screen.

public sealed partial class SettingsWindow : ProxySettingsWindow
{
    private readonly QuickSearchWidget _componentSearchWidget = new();
    private readonly CommandSettings _commandSettingsInstance = new();
    private readonly Dictionary<string, IReadOnlyList<ModSettingDrawer>> _componentSettingDrawerMap = [];
    private readonly MoralitySettings _moralitySettingsInstance = new();
    private readonly PawnSettings _pawnSettingsInstance = new();
    private readonly PointSettings _pointSettingsInstance = new();
    private readonly PollSettings _pollSettingsInstance = new();
    private readonly StoreSettings _storeSettingsInstance = new();
    private readonly TabularDrawer _tabWorker;
    private Vector2 _commandScrollPosition = Vector2.zero;
    private Vector2 _componentContentScrollPosition = Vector2.zero;
    private IComponent? _selectedComponent = null;
    private Vector2 _componentListScrollPosition = Vector2.zero;
    private Vector2 _moralityScrollPosition = Vector2.zero;
    private Vector2 _pawnScrollPosition = Vector2.zero;
    private Vector2 _pointScrollPosition = Vector2.zero;
    private Vector2 _pollScrollPosition = Vector2.zero;
    private Vector2 _storeScrollPosition = Vector2.zero;
    private IList<IComponent> _componentWorkingList = Registries.ComponentRegistry.AllRegistrants;

    /// <inheritdoc />
    public SettingsWindow(Verse.Mod mod) : base(mod)
    {
        doCloseX = false;

        for (var index = 0; index < _componentWorkingList.Count; index++)
        {
            IComponent component = _componentWorkingList[index];

            if (component.Settings != null)
            {
                _componentSettingDrawerMap[component.Id] = ModSettingFactory.FromInstance(component.Settings);
            }
        }

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
                    o.Icon = Icons.Cube;
                    o.Layout = IconLayout.IconAndText;
                    o.Label = "Components".MarkNotTranslated();
                    o.Tooltip = "A collection of settings that affects how the mod's modular components function.".MarkNotTranslated();
                    o.Drawer = DrawComponentSettings;
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

        _tabWorker.Draw(RectExtensions.AtZero(ref inRect));

        GUI.EndGroup();
    }

    private static void DrawSetting(Rect region, ModSettingDrawer setting)
    {
        (Rect labelRegion, Rect fieldRegion) = region.Split();

        if (setting.Experimental)
        {
            Rect warningIconRegion = LayoutHelper.IconRect(labelRegion.x, labelRegion.y, labelRegion.height, labelRegion.height, 6f);

            labelRegion.SetX(labelRegion.x + labelRegion.height);
            labelRegion.SetWidth(labelRegion.width - labelRegion.height);

            IconDrawer.DrawIcon(warningIconRegion, Icons.TriangleExclamation, DescriptionDrawer.ExperimentalTextColor);

            if (Mouse.IsOver(warningIconRegion))
            {
                // TODO: This allocates a new string every draw frame while the mouse is hovered over the icon.
                //       This should be cached since it's unlikely users will ever change the game's current language,
                //       but in the event they do, the mod can hook into the game's translations system to detect
                //       language changes.
                TooltipHandler.TipRegion(warningIconRegion, DescriptionDrawer.ExperimentalNoticeText.ColorTagged(DescriptionDrawer.ExperimentalTextColor));
            }
        }

        LabelDrawer.Draw(labelRegion, setting.Label);
        setting.Drawer.Draw(ref fieldRegion);
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

            if (descriptionRegion.IsVisible(viewport, scrollPosition))
            {
                DescriptionDrawer.DrawDescription(descriptionRegion, setting.Description!);
            }
        }

        GUI.EndScrollView();
        GUI.EndGroup();
    }

    private void DrawComponentSettings(Rect region)
    {
        var searchRegion = new Rect(region.x, region.y, region.width * 0.35f, UiConstants.LineHeight);
        var listRegion = new Rect(region.x, region.y + UiConstants.LineHeight + 5f, searchRegion.width, region.height - UiConstants.LineHeight - 5f);
        var contentRegion = new Rect(listRegion.width + 10f, region.y, region.width - listRegion.width - 10f, region.height);

        GUI.BeginGroup(region);

        GUI.BeginGroup(searchRegion);
        _componentSearchWidget.OnGUI(GenUI.AtZero(searchRegion), FilterComponentList);
        GUI.EndGroup();

        GUI.BeginGroup(listRegion);
        DrawComponentList(RectExtensions.AtZero(ref listRegion));
        GUI.EndGroup();

        GUI.BeginGroup(contentRegion);

        if (_selectedComponent != null && _componentSettingDrawerMap.TryGetValue(_selectedComponent.Id, out IReadOnlyList<ModSettingDrawer>? drawers))
        {
            DrawTabSettings(RectExtensions.AtZero(ref contentRegion), drawers, ref _componentContentScrollPosition);
        }

        GUI.EndGroup();

        GUI.EndGroup();
    }

    private void DrawComponentList(Rect region)
    {
        Rect innerListRegion = GenUI.ContractedBy(region, 5f);

        Widgets.DrawMenuSection(region);

        GUI.BeginGroup(innerListRegion);
        RectExtensions.AtZero(ref innerListRegion);

        int totalComponents = _componentWorkingList.Count;
        float scrollViewHeight = UiConstants.LineHeight * totalComponents;
        var viewport = new Rect(0f, 0f, innerListRegion.width - (scrollViewHeight > innerListRegion.height ? 16f : 0f), scrollViewHeight);
        _componentListScrollPosition = GUI.BeginScrollView(innerListRegion, _componentListScrollPosition, viewport);

        var yPosition = 0f;

        for (var index = 0; index < _componentWorkingList.Count; index++)
        {
            var lineRegion = new Rect(0f, yPosition, viewport.width, UiConstants.LineHeight);
            yPosition += lineRegion.height;

            if (!lineRegion.IsVisible(viewport, _componentListScrollPosition))
            {
                continue;
            }

            IComponent component = _componentWorkingList[index];
            LabelDrawer.Draw(lineRegion, component.Name);

            if (Mouse.IsOver(lineRegion))
            {
                Widgets.DrawLightHighlight(lineRegion);
            }

            if (_selectedComponent == component)
            {
                Widgets.DrawHighlightSelected(lineRegion);
            }

            if (!Widgets.ButtonInvisible(lineRegion))
            {
                continue;
            }

            _selectedComponent = component;
            _componentContentScrollPosition.AtZero();
        }

        GUI.EndScrollView();
        GUI.EndGroup();
    }

    private void FilterComponentList()
    {
        var copy = new List<IComponent>();
        IList<IComponent> components = Registries.ComponentRegistry.AllRegistrants;

        for (var index = 0; index < components.Count; index++)
        {
            IComponent component = components[index];

            if (string.Compare(_componentSearchWidget.filter.Text, component.Name, StringComparison.OrdinalIgnoreCase) > 0)
            {
                copy.Add(component);
            }
            else if (component == _selectedComponent)
            {
                _selectedComponent = null;
            }
        }

        _componentWorkingList = copy.AsReadOnly();
    }
}
