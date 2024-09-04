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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using HarmonyLib;
using RimWorld;
using StreamKit.Mod.Shared.Logging;
using UnityEngine;
using Verse;
using Logger = NLog.Logger;

namespace StreamKit.Mod.Bootstrap;

[StaticConstructorOnStartup]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
internal static class Bootstrap
{
    private static readonly Logger Logger = KitLogManager.GetLogger("StreamKit.Bootstrapper");
    private static readonly string? NativeExtension = GetNativeExtension();
    private static readonly XmlSerializer Serializer = new(typeof(Corpus));
    private static readonly List<string> SpecialFiles = [];

    static Bootstrap()
    {
        Logger.Info("StreamKit is running on the platform '{Platform}'", UnityData.platform);

        if (string.IsNullOrEmpty(NativeExtension))
        {
            Logger.Fatal("'{Platform}' is an unsupported platform; Aborting initialization...", UnityData.platform);

            return;
        }

        DisableHarmonyStacktraceCaching();
        DisableHarmonyStacktraceEnhancing();

        Application.wantsToQuit += CleanNativeFiles;

        foreach (ModContentPack mod in LoadedModManager.RunningMods)
        {
            string corpusPath = Path.Combine(mod.RootDir, "Corpus.xml");

            if (!File.Exists(corpusPath))
            {
                continue;
            }

            LoadContent(mod, corpusPath);
        }
    }

    [Conditional("DEBUG")]
    private static void DisableHarmonyStacktraceCaching()
    {
        try
        {
            ref bool cachingField = ref AccessTools.StaticFieldRefAccess<bool>("HarmonyMod.HarmonyMain:noStacktraceCaching");
            cachingField = true;
        }
        catch (Exception e)
        {
            Logger.Error(e, "Could not toggle Harmony's, the RimWorld mod, stacktrace caching.");
        }
    }

    [Conditional("DEBUG")]
    private static void DisableHarmonyStacktraceEnhancing()
    {
        try
        {
            ref bool prettifierField = ref AccessTools.StaticFieldRefAccess<bool>("HarmonyMod.HarmonyMain:noStacktraceEnhancing");
            prettifierField = true;
        }
        catch (Exception e)
        {
            Logger.Error(e, "Could not toggle Harmony's, the RimWorld mod, stacktrace enhancing");
        }
    }

    private static bool CleanNativeFiles()
    {
        for (var index = 0; index < SpecialFiles.Count; index++)
        {
            string file = SpecialFiles[index];

            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Could not clean file @ {File}", file);
                Logger.Warn("Any mod updates pending to {File} may not go through next relaunch", file);
            }
        }

        return true;
    }

    private static void LoadContent(ModContentPack mod, string corpusPath)
    {
        Corpus? corpus;

        using (FileStream stream = File.Open(corpusPath, FileMode.Open, FileAccess.Read))
        {
            corpus = Serializer.Deserialize(stream) as Corpus;
        }

        if (corpus is null)
        {
            Logger.Error("Object within corpus file for {ModName} was malformed. Aborting...", mod.Name);

            return;
        }

        LoadResources(mod, corpus);
    }

    private static void LoadResources(ModContentPack mod, Corpus corpus)
    {
        foreach (ResourceBundle bundle in corpus.Resources)
        {
            LoadResourceBundle(mod, bundle);
        }
    }

    private static void LoadResourceBundle(ModContentPack mod, ResourceBundle bundle)
    {
        string path = GetPathFor(mod, bundle);

        if (!Directory.Exists(path))
        {
            Logger.Error("The directory {Path} doesn't exist, but was specified in {ModName}'s corpus. Aborting...", path, mod.Name);

            return;
        }

        var assemblyCandidates = new List<Assembly>();

        foreach (Resource resource in bundle.Resources)
        {
            string resourceDir = string.IsNullOrEmpty(resource.Root) ? Path.GetFullPath(path) : Path.GetFullPath(Path.Combine(path, resource.Root));

            switch (resource.Type)
            {
                case ResourceType.Dll:
                    CopyNativeFile(resource, resourceDir);

                    break;
                case ResourceType.Assembly:
                    Assembly? assembly = BootModLoader.LoadAssembly(
                        mod,
                        string.IsNullOrEmpty(resource.Root)
                            ? Path.Combine(path, "Assemblies", $"{resource.Name}.dll")
                            : Path.Combine(path, resource.Root, "Assemblies", $"{resource.Name}.dll")
                    );

                    if (assembly == null)
                    {
                        break;
                    }

                    assemblyCandidates.Add(assembly);

                    break;
                case ResourceType.NetStandardAssembly:
                    CopyStandardManagedFile(resource, resourceDir);

                    break;
                default:
                    Logger.Warn(@"Cannot load resource ""{Resource}"" due to an unsupported type of ""{ResourceType}""", resource.Name, resource.Type.ToStringFast());

                    break;
            }
        }

        if (assemblyCandidates.Count <= 0)
        {
            return;
        }

        foreach (Assembly assembly in assemblyCandidates)
        {
            try
            {
                BootModLoader.InstantiateModClasses(mod, assembly);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Encountered one or more errors while instantiating mod classes for assembly {Assembly} from {ModName}", assembly.GetName(), mod.PackageId);
            }

            try
            {
                BootModLoader.RunStaticConstructors(assembly);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Encounter one or more errors while running static constructors for assembly {Assembly} from {ModName}", assembly.GetName(), mod.PackageId);
            }
        }
    }

    private static void CopyNativeFile(Resource resource, string resourceDir)
    {
        var fileName = $"{resource.Name}.{NativeExtension}";
        string resourcePath = Path.Combine(resourceDir, $"{resource.Name}.{NativeExtension}");
        string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        if (File.Exists(destinationPath) || (!File.Exists(resourcePath) && resource.Optional))
        {
            return;
        }

        try
        {
            File.Copy(resourcePath, destinationPath);

            SpecialFiles.Add(destinationPath);
        }
        catch (Exception e)
        {
            Logger.Error(
                e,
                "Could not copy {FileName} to {DestinationPath} (from {ResourcePath}). Things will not work correctly",
                fileName,
                destinationPath,
                resourcePath
            );
        }
    }


    private static void CopyStandardManagedFile(Resource resource, string resourceDir)
    {
        var fileName = $"{resource.Name}.dll";
        string resourcePath = Path.Combine(resourceDir, $"{resource.Name}.dll");
        string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        if (File.Exists(destinationPath))
        {
            return;
        }

        try
        {
            File.Copy(resourcePath, destinationPath);

            SpecialFiles.Add(destinationPath);
        }
        catch (Exception e)
        {
            Logger.Error(
                e,
                "Could not copy {FileName} to {DestinationPath} (from {ResourcePath}). Things will not work correctly",
                fileName,
                destinationPath,
                resourcePath
            );
        }
    }

    private static string GetPathFor(ModContentPack mod, ResourceBundle bundle)
    {
        string root = mod.RootDir;

        if (!string.IsNullOrEmpty(bundle.Root))
        {
            root = Path.Combine(root, bundle.Root);
        }

        if (!bundle.Versioned)
        {
            return root;
        }

        string withoutBuild = Path.Combine(root, VersionControl.CurrentVersionString);

        return Directory.Exists(withoutBuild) ? withoutBuild : Path.Combine(root, VersionControl.CurrentVersionStringWithoutBuild);
    }

    private static string? GetNativeExtension()
    {
        return UnityData.platform switch
        {
            RuntimePlatform.WindowsPlayer => ".dll",
            RuntimePlatform.LinuxPlayer => ".so",
            RuntimePlatform.OSXPlayer => ".dylib",
            var _ => null
        };
    }
}
