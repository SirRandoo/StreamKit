using JetBrains.Annotations;
using SirRandoo.CommonLib.Helpers;
using UnityEngine;
using Verse;

namespace StreamKit.Bootstrap;

[StaticConstructorOnStartup]
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal class BootstrapMod(ModContentPack content) : Mod(content)
{
    private bool _isKitLoaded;

    /// <inheritdoc/>
    public override string? SettingsCategory()
    {
        if (!_isKitLoaded && ModLister.GetActiveModWithIdentifier("sirrandoo.streamkit") is null)
        {
            return "StreamKit - Bootstrapper";
        }

        _isKitLoaded = true;

        return null;
    }

    /// <inheritdoc/>
    public override void DoSettingsWindowContents(Rect inRect)
    {
        UiHelper.Label(inRect, "StreamKit.Bootstrap.Info".TranslateSimple());
    }
}
