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
using System.IO;
using HarmonyLib;
using JetBrains.Annotations;
using NLog;
using StreamKit.Mod.Api;
using Verse;

namespace StreamKit.Mod.Shared.Core;

/// <summary>
///     A small class for loading component settings.
/// </summary>
[UsedImplicitly]
[StaticConstructorOnStartup]
internal static class ComponentOperations
{
    private static readonly Logger Logger = KitLogManager.GetLogger("StreamKit.Operations.ComponentSettings");

    static ComponentOperations()
    {
        foreach (IComponent component in Registries.ComponentRegistry.AllRegistrants)
        {
            string componentSettingsPath = FilePaths.GetSettingsPath(component);
            string componentSettingsDirectory = Directory.GetDirectoryRoot(componentSettingsPath);

            if (!Directory.Exists(componentSettingsDirectory))
            {
                try
                {
                    Directory.CreateDirectory(componentSettingsDirectory);
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Could not create directory at {Directory} :: Component settings will be aborted", componentSettingsDirectory);

                    return;
                }
            }

            component.Settings = component.SettingsProvider.TryLoadSettings(componentSettingsPath, out IComponentSettings? settings)
                ? settings
                : component.SettingsProvider.GenerateDefaultSettings();

            Logger.Trace("Loaded settings for component {Component} :: Loaded settings type is {Settings}", component.Name, component.Settings.GetType().FullDescription());
        }
    }
}
