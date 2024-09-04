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

using System.Linq;
using System.Reflection;
using System.Text;
using SirRandoo.UX;
using SirRandoo.UX.Drawers;
using SirRandoo.UX.Helpers;
using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;
using UnityEngine;
using Verse;
using DescriptionAttribute = StreamKit.Mod.Api.Attributes.DescriptionAttribute;

namespace StreamKit.Mod.Core.Windows;

internal class RuntimeFlagWindow : Window
{
    private const float WindowWidth = 350;
    private const float WindowHeight = 400;
    private const float LineSplitPercent = 0.9f;

    private static bool _staticStateSet;
    private static RuntimeFlag[] _allFlags = null!;
    private static int _totalFlags;
    private static float _viewportHeight;
    private Vector2 _scrollPosition = Vector2.zero;

    private RuntimeFlagWindow()
    {
    }

    /// <inheritdoc />
    public override Vector2 InitialSize => new(WindowWidth, WindowHeight);

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        var viewport = new Rect(0f, 0f, inRect.width - (_viewportHeight > inRect.height ? 16f : 0f), _viewportHeight);

        GUI.BeginGroup(inRect);
        _scrollPosition = GUI.BeginScrollView(inRect, _scrollPosition, viewport);

        for (var i = 0; i < _totalFlags; i++)
        {
            RuntimeFlag flag = _allFlags[i];

            var lineRegion = new Rect(0f, UiConstants.LineHeight * i, viewport.width, UiConstants.LineHeight);

            if (!lineRegion.IsVisible(inRect, _scrollPosition))
            {
                continue;
            }

            if (i % 1 == 0)
            {
                Widgets.DrawLightHighlight(lineRegion);
            }

            bool flagState = RuntimeConfig.HasFlag(flag.Flag);
            (Rect labelRegion, Rect checkboxRegion) = lineRegion.Split(LineSplitPercent);
            LabelDrawer.DrawLabel(labelRegion, flag.Flag);

            GUI.DrawTexture(
                LayoutHelper.IconRect(checkboxRegion.x, checkboxRegion.y, checkboxRegion.width, checkboxRegion.height),
                flagState ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex
            );

            if (Widgets.ButtonInvisible(lineRegion))
            {
                RuntimeConfig.ToggleFlag(flag.Flag);
            }

            if (!string.IsNullOrEmpty(flag.Description))
            {
                TooltipHandler.TipRegion(lineRegion, flag.Description);
            }

            if (flag.IsExperimental)
            {
                TooltipHandler.TipRegion(lineRegion, UxLocale.ExperimentalNoticeColored);
            }
        }

        GUI.EndScrollView();
        GUI.EndGroup();
    }

    /// <inheritdoc />
    public override void PostClose()
    {
        RuntimeConfigWorker.Save();
    }

    internal static RuntimeFlagWindow CreateInstance()
    {
        if (_staticStateSet)
        {
            return new RuntimeFlagWindow();
        }

        _allFlags = typeof(RuntimeConfig.RuntimeFlags).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
           .Where(f => f.IsLiteral && !f.IsInitOnly)
           .Select(BuildFlag)
           .ToArray();

        _totalFlags = _allFlags.Length;
        _viewportHeight = _totalFlags * UiConstants.LineHeight;

        _staticStateSet = true;

        return new RuntimeFlagWindow();
    }

    private static RuntimeFlag BuildFlag(FieldInfo flagField)
    {
        bool isExperimental = flagField.HasAttribute<ExperimentalAttribute>();
        string description = GetFlagDescription(flagField);

        if (!string.IsNullOrEmpty(description))
        {
            description = description.ColorTagged(DescriptionDrawer.DescriptionTextColor);
        }

        return new RuntimeFlag((string)flagField.GetRawConstantValue(), description, isExperimental);
    }

    private static string GetFlagDescription(FieldInfo field)
    {
        if (!field.HasAttribute<DescriptionAttribute>())
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        foreach (DescriptionAttribute? attribute in field.GetCustomAttributes<DescriptionAttribute>())
        {
            if (builder.Length > 0)
            {
                builder.Append(!attribute.Inline ? "\n" : " ");
            }

            builder.Append(attribute.IsKey ? attribute.Text.TranslateSimple() : attribute.Text);
        }

        return builder.ToString();
    }

    private sealed record RuntimeFlag(string Flag, string Description, bool IsExperimental = false);
}
