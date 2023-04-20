using JetBrains.Annotations;
using SirRandoo.CommonLib.Helpers;
using UnityEngine;
using Verse;

namespace StreamKit.Bootstrap
{
    [StaticConstructorOnStartup]
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
    internal class BootstrapMod : Mod
    {
        private bool _isKitLoaded;

        /// <inheritdoc/>
        public BootstrapMod(ModContentPack content) : base(content)
        {
            Instance = this;
        }

        internal static BootstrapMod Instance { get; private set; }

        /// <inheritdoc/>
        [CanBeNull]
        public override string SettingsCategory()
        {
            if (!_isKitLoaded && ModLister.GetActiveModWithIdentifier("sirrandoo.streamkit") == null)
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
}
