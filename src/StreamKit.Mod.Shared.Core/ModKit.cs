using StreamKit.Mod.Shared.Core.Windows;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Shared.Core;

public class ModKit : Verse.Mod
{
    public ModKit(ModContentPack content) : base(content)
    {
        Instance = this;
        SettingsWindow = new SettingsWindow(this);
    }

    public static ModKit Instance { get; private set; } = null!;

    internal SettingsWindow SettingsWindow { get; set; }

    /// <inheritdoc />
    public override string SettingsCategory() => Content.Name;

    /// <inheritdoc />
    public override void DoSettingsWindowContents(Rect inRect)
    {
        ProxySettingsWindow.Open(SettingsWindow);
    }
}
