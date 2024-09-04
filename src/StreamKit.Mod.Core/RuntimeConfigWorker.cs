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

using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using NLog;
using StreamKit.Mod.Api;
using StreamKit.Mod.Shared;
using StreamKit.Mod.Shared.Logging;
using Verse;

namespace StreamKit.Mod.Core;

[StaticConstructorOnStartup]
internal static class RuntimeConfigWorker
{
    private static readonly Logger Logger = KitLogManager.GetLogger("StreamKit.Workers.RuntimeConfig");

    static RuntimeConfigWorker()
    {
        RegisterEventListeners();

        Load();
    }

    private static void Load()
    {
        if (!File.Exists(FilePaths.ActiveFlagsFile))
        {
            return;
        }

        string[] activeFlags;

        try
        {
            activeFlags = File.ReadAllLines(FilePaths.ActiveFlagsFile);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Could not read previously active runtime flags");

            return;
        }

        for (var i = 0; i < activeFlags.Length; i++)
        {
            RuntimeConfig.SetFlag(activeFlags[i]);
        }
    }

    internal static void Save()
    {
        IReadOnlyList<string> allFlags = RuntimeConfig.AllFlags;

        try
        {
            File.WriteAllLines(FilePaths.ActiveFlagsFile, allFlags);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Could not save active runtime flags to disk; they will not persist between sessions");
        }
    }

    private static void RegisterEventListeners()
    {
        RuntimeConfig.RuntimeFlagChanged += OnHarmonyStacktraceCachingChanged;
        RuntimeConfig.RuntimeFlagChanged += OnHarmonyStacktraceEnhancingChanged;
    }

    private static void OnHarmonyStacktraceCachingChanged(object _, RuntimeFlagChangedEventArgs eventArgs)
    {
        if (eventArgs.Flag != RuntimeConfig.RuntimeFlags.DisableHarmonyStacktraceCaching)
        {
            return;
        }

        try
        {
            ref bool cachingField = ref AccessTools.StaticFieldRefAccess<bool>("HarmonyMod.HarmonyMain:noStacktraceCaching");
            cachingField = eventArgs.Active;
        }
        catch (Exception e)
        {
            Logger.Error(e, "Could not toggle Harmony's, the RimWorld mod, stacktrace caching.");
        }
    }

    private static void OnHarmonyStacktraceEnhancingChanged(object _, RuntimeFlagChangedEventArgs eventArgs)
    {
        if (eventArgs.Flag != RuntimeConfig.RuntimeFlags.DisableHarmonyStacktraceEnhancing)
        {
            return;
        }

        try
        {
            ref bool prettifierField = ref AccessTools.StaticFieldRefAccess<bool>("HarmonyMod.HarmonyMain:noStacktraceEnhancing");
            prettifierField = eventArgs.Active;
        }
        catch (Exception e)
        {
            Logger.Error(e, "Could not toggle Harmony's, the RimWorld mod, stacktrace enhancing");
        }
    }
}
