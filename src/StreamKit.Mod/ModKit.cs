using JetBrains.Annotations;
using SirRandoo.CommonLib;
using SirRandoo.CommonLib.Windows;
using Verse;

namespace StreamKit.Mod;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal class ModKit(ModContentPack content) : ModPlus(content)
{
    /// <inheritdoc/>
    public override ProxySettingsWindow SettingsWindow => new SettingsWindow(this);
}
