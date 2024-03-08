using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using Verse;

namespace StreamKit.Bootstrap.Shared.Core;

[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
public static class BootModLoader
{
    private static readonly Dictionary<Type, Mod> RunningModClassesField =
        AccessTools.StaticFieldRefAccess<Dictionary<Type, Mod>>("Verse.LoadedModManager:runningModClasses");

    private static readonly MethodInfo AssemblyIsUsableMethodInfo = AccessTools.Method("Verse.ModAssemblyHandler:AssemblyIsUsable");

    private static readonly Func<ModAssemblyHandler, Assembly, bool> AssemblyIsUsableMethod =
        (Func<ModAssemblyHandler, Assembly, bool>)Delegate.CreateDelegate(typeof(Func<ModAssemblyHandler, Assembly, bool>), AssemblyIsUsableMethodInfo);

    /// <summary>
    ///     Loads an assembly, and its PDB file if it exists.
    /// </summary>
    /// <param name="pack">The mod data being associated with the assembly.</param>
    /// <param name="path">The path to the assembly being loaded.</param>
    /// <returns>The assembly that was loaded from the <see cref="path" />.</returns>
    public static Assembly? LoadAssembly(ModContentPack pack, string path)
    {
        if (!File.Exists(path))
        {
            Log.Error($"[StreamKit Bootstrapper] Could not load non-existent assembly at path {path} ; Aborting...");

            return null;
        }

        string pdbPath = Path.ChangeExtension(path, ".pdb");
        byte[] content = File.ReadAllBytes(path);
        byte[] pdbContent = File.Exists(pdbPath) ? File.ReadAllBytes(pdbPath) : [];

        Assembly assembly;

        try
        {
            assembly = AppDomain.CurrentDomain.Load(content, pdbContent);
        }
        catch (Exception e)
        {
            Log.Error($"[StreamKit Bootstrap] Could not load assembly @ {path}\n\n{e}");

            return null;
        }

        AssemblyIsUsableMethod(pack.assemblies, assembly);
        pack.assemblies.loadedAssemblies.Add(assembly);

        try
        {
            InstantiateModClasses(pack, assembly);
        }
        catch (ReflectionTypeLoadException e)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < e.LoaderExceptions.Length; i++)
            {
                builder.Append(e.LoaderExceptions[i]).Append("\n\n");
            }

            if (builder.Length > 0)
            {
                builder.Insert(0, $"Encountered one or more errors while instantiating mod classes for {pack.Name} ({pack.PackageId}) in assembly {assembly.GetName()}:\n");
            }
        }

        try
        {
            RunStaticConstructors(assembly);
        }
        catch (Exception e)
        {
            Log.Error($"[StreamKit Bootstrapper] Encountered an error while running static constructors for {pack.Name}'s assembly @ {path}\n\n{e.InnerException ?? e}");
        }

        return assembly;
    }

    /// <summary>
    ///     Runs the static constructors of all types annotated with
    ///     <see cref="StaticConstructorOnStartup" /> in an assembly.
    /// </summary>
    /// <param name="assembly">
    ///     An assembly with types annotated with
    ///     <see cref="StaticConstructorOnStartup" />.
    /// </param>
    public static void RunStaticConstructors(Assembly assembly)
    {
        try
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.TryGetAttribute<StaticConstructorOnStartup>() == null)
                {
                    continue;
                }

                RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            }
        }
        catch (ReflectionTypeLoadException e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"ReflectionTypeLoadException getting types in assembly {assembly.GetName().Name}: {e}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Loader exceptions:");

            if (e.LoaderExceptions != null)
            {
                Exception[] loaderExceptions = e.LoaderExceptions;

                foreach (Exception ex2 in loaderExceptions)
                {
                    stringBuilder.AppendLine("   => " + ex2);
                }
            }

            Log.Error(stringBuilder.Insert(0, "[StreamKit Bootstrapper] ").ToString());
        }
    }

    /// <summary>
    ///     Instantiates <see cref="Mod" /> classes, and adds them to an
    ///     internal field in RimWorld's <see cref="LoadedModManager" />
    ///     class.
    /// </summary>
    /// <param name="mod">The mod the assembly belongs to.</param>
    /// <param name="assembly">
    ///     The assembly being indexed for
    ///     <see cref="Mod" /> classes.
    /// </param>
    public static void InstantiateModClasses(ModContentPack mod, Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            try
            {
                if (typeof(Mod).IsAssignableFrom(type) && Activator.CreateInstance(type, mod) is Mod instance)
                {
                    RunningModClassesField.Add(type, instance);
                }
            }
            catch (Exception e)
            {
                Log.Error($"[StreamKit Bootstrapper] Could not instantiate {type.FullDescription()}\n\n{e}");
            }
        }
    }
}
