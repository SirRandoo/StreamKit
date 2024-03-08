using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;
using RimWorld;
using UnityEngine;
using Verse;

namespace StreamKit.Bootstrap.Shared.Core;

[StaticConstructorOnStartup]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
internal static class Bootstrap
{
    private static readonly string? NativeExtension = GetNativeExtension();
    private static readonly XmlSerializer Serializer = new(typeof(Corpus));
    private static readonly List<string> SpecialFiles = [];

    static Bootstrap()
    {
        if (string.IsNullOrEmpty(NativeExtension))
        {
            Log.Error($"[StreamKit Bootstrapper] Bootstrap is running on an unsupported platform '{UnityData.platform.ToStringSafe()}'. Aborting...");

            return;
        }

        Application.wantsToQuit += CleanNativeFiles;

        Log.Message($"[StreamKit Bootstrapper] StreamKit is running on the platform '{UnityData.platform.ToStringSafe()}'.");

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
                Log.Error($"[StreamKit Bootstrapper] Could not clean file @ {file}. Any mod updates pending to this file will not go through.\n\n{e}");
            }
        }

        return true;
    }

    private static void LoadContent(ModContentPack mod, string corpusPath)
    {
        if (!File.Exists(corpusPath))
        {
            Log.Error($"[StreamKit Bootstrapper] {mod.Name} requested that content be loaded, but doesn't have a corpus file in their root directory. Aborting...");

            return;
        }

        Corpus? corpus;

        using (FileStream stream = File.Open(corpusPath, FileMode.Open, FileAccess.Read))
        {
            corpus = Serializer.Deserialize(stream) as Corpus;
        }

        if (corpus is null)
        {
            Log.Error($"[StreamKit Bootstrapper] Object within corpus file for {mod.Name} was malformed. Aborting...");

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
            Log.Error($"[StreamKit Bootstrapper] The directory {path} doesn't exist, but was specified in {mod.Name}'s corpus. Aborting...");

            return;
        }

        foreach (Resource resource in bundle.Resources)
        {
            string resourceDir = Path.GetFullPath(Path.Combine(path, resource.Root));

            switch (resource.Type)
            {
                case ResourceType.Dll:
                    CopyNativeFile(resource, resourceDir);

                    break;
                case ResourceType.Assembly:
                    BootModLoader.LoadAssembly(mod, Path.Combine(path, resource.Root, $"{resource.Name}.dll"));

                    break;
                case ResourceType.NetStandardAssembly:
                    CopyStandardManagedFile(resource, resourceDir);

                    break;
            }
        }
    }

    private static void CopyNativeFile(Resource resource, string resourceDir)
    {
        var fileName = $"{resource.Name}.{NativeExtension}";
        string resourcePath = Path.Combine(resourceDir, $"{resource.Name}.{NativeExtension}");
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
            Log.Error($"[StreamKit Bootstrapper] Could not copy {fileName} to {destinationPath} (from {resourcePath}). Things will not work correctly.\n\n{e}");
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
            Log.Error($"[StreamKit Bootstrapper] Could not copy {fileName} to {destinationPath} (from {resourcePath}). Things will not work correctly.\n\n{e}");
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
        switch (UnityData.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                return ".dll";
            case RuntimePlatform.LinuxPlayer:
                return ".so";
            case RuntimePlatform.OSXPlayer:
                return ".dylib";
            default:
                return null;
        }
    }
}
