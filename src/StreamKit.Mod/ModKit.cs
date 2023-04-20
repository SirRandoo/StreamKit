using JetBrains.Annotations;
using SirRandoo.CommonLib;
using SirRandoo.CommonLib.Windows;
using Verse;

namespace StreamKit.Mod
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
    internal class ModKit : ModPlus
    {
        /// <inheritdoc/>
        public ModKit(ModContentPack content) : base(content)
        {
        }

        /// <inheritdoc/>
        [NotNull]
        public override ProxySettingsWindow SettingsWindow => new SettingsWindow(this);
    }
}
