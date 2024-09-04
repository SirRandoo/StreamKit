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

using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Bootstrap;

[StaticConstructorOnStartup]
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal class BootstrapMod : Verse.Mod
{
    private static readonly Color DescriptionTextColor = new(0.72f, 0.72f, 0.72f);

    public BootstrapMod(ModContentPack content) : base(content)
    {
        Instance = this;
    }

    public static BootstrapMod Instance { get; private set; } = null!;

    /// <inheritdoc />
    public override string? SettingsCategory() => ModLister.GetActiveModWithIdentifier("com.sirrandoo.streamkit") != null ? "StreamKit - Bootstrapper" : null;

    /// <inheritdoc />
    public override void DoSettingsWindowContents(Rect inRect)
    {
        Color oldColor = GUI.color;
        GameFont oldFont = Text.Font;
        TextAnchor oldAnchor = Text.Anchor;

        Text.Font = GameFont.Medium;
        GUI.color = DescriptionTextColor;
        Text.Anchor = TextAnchor.MiddleCenter;

        Widgets.Label(inRect, "StreamKit.Bootstrap.Info".TranslateSimple());

        Text.Font = oldFont;
        GUI.color = oldColor;
        Text.Anchor = oldAnchor;
    }
}
