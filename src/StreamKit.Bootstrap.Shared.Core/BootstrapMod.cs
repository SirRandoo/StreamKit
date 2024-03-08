using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace StreamKit.Bootstrap.Shared.Core;

[StaticConstructorOnStartup]
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal class BootstrapMod(ModContentPack content) : Mod(content)
{
    private static readonly Color DescriptionTextColor = new(0.72f, 0.72f, 0.72f);
    private bool _isKitLoaded;

    /// <inheritdoc />
    public override string? SettingsCategory()
    {
        if (!_isKitLoaded && ModLister.GetActiveModWithIdentifier("sirrandoo.streamkit") is null)
        {
            return "StreamKit - Bootstrapper";
        }

        _isKitLoaded = true;

        return null;
    }

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
