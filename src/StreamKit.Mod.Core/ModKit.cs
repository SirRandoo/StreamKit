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
using StreamKit.Mod.Api;
using StreamKit.Mod.Core.Settings;
using StreamKit.Mod.Core.Windows;
using StreamKit.Mod.Shared;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core;

public sealed class ModKit : Verse.Mod
{
    public ModKit(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = ModKitSettings.Load();
    }

    [PublicAPI] public static ModKit Instance { get; private set; } = null!;
    [PublicAPI] public ModKitSettings Settings { get; }

    internal SettingsWindow SettingsWindow => new(this);

    /// <inheritdoc />
    public override string SettingsCategory() => Content.Name;

    /// <inheritdoc />
    public override void DoSettingsWindowContents(Rect inRect)
    {
        ProxySettingsWindow.Open(SettingsWindow);
    }

    /// <inheritdoc />
    public override void WriteSettings()
    {
        Settings.TrySaveClientSettings();
        Settings.TrySaveCommandSettings();
        Settings.TrySaveMoralitySettings();
        Settings.TrySavePawnSettings();
        Settings.TrySavePointSettings();
        Settings.TrySavePollSettings();
        Settings.TrySaveStoreSettings();
    }
}

public sealed class ModKitSettings
{
    private static readonly string ClientSettingsPath = FilePaths.GetSettingsFile("streamkit.client.json");
    private static readonly string CommandSettingsPath = FilePaths.GetSettingsFile("streamkit.command.json");
    private static readonly string MoralitySettingsPath = FilePaths.GetSettingsFile("streamkit.morality.json");
    private static readonly string PawnSettingsPath = FilePaths.GetSettingsFile("streamkit.pawns.json");
    private static readonly string PointSettingsPath = FilePaths.GetSettingsFile("streamkit.points.json");
    private static readonly string PollSettingsPath = FilePaths.GetSettingsFile("streamkit.poll.json");
    private static readonly string StoreSettingsPath = FilePaths.GetSettingsFile("streamkit.store.json");

    private readonly SettingsProvider<ClientSettings> _clientSettingsProvider = new("Core.Client", "StreamKit.Settings.Client");
    private readonly SettingsProvider<CommandSettings> _commandSettingsProvider = new("Core.Command", "StreamKit.Settings.Command");
    private readonly SettingsProvider<MoralitySettings> _moralitySettingsProvider = new("Core.Morality", "StreamKit.Settings.Morality");
    private readonly SettingsProvider<PawnSettings> _pawnSettingsProvider = new("Core.Pawn", "StreamKit.Settings.Pawn");
    private readonly SettingsProvider<PointSettings> _pointSettingsProvider = new("Core.Point", "StreamKit.Settings.Point");
    private readonly SettingsProvider<PollSettings> _pollSettingsProvider = new("Core.Poll", "StreamKit.Settings.Poll");
    private readonly SettingsProvider<StoreSettings> _storeSettingsProvider = new("Core.Store", "StreamKit.Settings.Store");

    public ClientSettings Client { get; set; } = null!;
    public CommandSettings Command { get; set; } = null!;
    public MoralitySettings Morality { get; set; } = null!;
    public PawnSettings Pawn { get; set; } = null!;
    public PointSettings Point { get; set; } = null!;
    public PollSettings Poll { get; set; } = null!;
    public StoreSettings Store { get; set; } = null!;

    public bool TrySaveClientSettings() => _clientSettingsProvider.TrySaveSettings(ClientSettingsPath, Client);
    public bool TrySaveCommandSettings() => _commandSettingsProvider.TrySaveSettings(CommandSettingsPath, Command);
    public bool TrySaveMoralitySettings() => _moralitySettingsProvider.TrySaveSettings(MoralitySettingsPath, Morality);
    public bool TrySavePawnSettings() => _pawnSettingsProvider.TrySaveSettings(PawnSettingsPath, Pawn);
    public bool TrySavePointSettings() => _pointSettingsProvider.TrySaveSettings(PointSettingsPath, Point);
    public bool TrySavePollSettings() => _pollSettingsProvider.TrySaveSettings(PollSettingsPath, Poll);
    public bool TrySaveStoreSettings() => _storeSettingsProvider.TrySaveSettings(StoreSettingsPath, Store);

    public static ModKitSettings Load()
    {
        var instance = new ModKitSettings();

        instance._clientSettingsProvider.TryLoadSettings(FilePaths.GetSettingsFile(ClientSettingsPath), out ClientSettings? clientSettings);
        instance.Client = clientSettings ?? instance._clientSettingsProvider.GenerateDefaultSettings();

        instance._commandSettingsProvider.TryLoadSettings(FilePaths.GetSettingsFile(CommandSettingsPath), out CommandSettings? commandSettings);
        instance.Command = commandSettings ?? instance._commandSettingsProvider.GenerateDefaultSettings();

        instance._moralitySettingsProvider.TryLoadSettings(FilePaths.GetSettingsFile(MoralitySettingsPath), out MoralitySettings? moralitySettings);
        instance.Morality = moralitySettings ?? instance._moralitySettingsProvider.GenerateDefaultSettings();

        instance._pawnSettingsProvider.TryLoadSettings(FilePaths.GetSettingsFile(PawnSettingsPath), out PawnSettings? pawnSettings);
        instance.Pawn = pawnSettings ?? instance._pawnSettingsProvider.GenerateDefaultSettings();

        instance._pointSettingsProvider.TryLoadSettings(FilePaths.GetSettingsFile(PointSettingsPath), out PointSettings? pointSettings);
        instance.Point = pointSettings ?? instance._pointSettingsProvider.GenerateDefaultSettings();

        instance._pollSettingsProvider.TryLoadSettings(FilePaths.GetSettingsFile(PollSettingsPath), out PollSettings? pollSettings);
        instance.Poll = pollSettings ?? instance._pollSettingsProvider.GenerateDefaultSettings();

        instance._storeSettingsProvider.TryLoadSettings(FilePaths.GetSettingsFile(StoreSettingsPath), out StoreSettings? storeSettings);
        instance.Store = storeSettings ?? instance._storeSettingsProvider.GenerateDefaultSettings();

        return instance;
    }
}
