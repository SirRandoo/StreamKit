// MIT License
// 
// Copyright (c) 2023 SirRandoo
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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using JetBrains.Annotations;
using SirRandoo.CommonLib.Entities;
using SirRandoo.CommonLib.Interfaces;
using UnityEngine;
using Verse;

namespace StreamKit.Bootstrap
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
    public static class BootModLoader
    {
        private static readonly IRimLogger Logger = new RimThreadedLogger("StreamKit.ModLoader");

        private static readonly Dictionary<Type, Mod> RunningModClassesField =
            AccessTools.StaticFieldRefAccess<Dictionary<Type, Mod>>("Verse.LoadedModManager:runningModClasses");
        private static readonly MethodInfo AssemblyIsUsableMethod = AccessTools.Method("Verse.ModAssemblyHandler:AssemblyIsUsable");

        /// <summary>
        ///     Loads an assembly, and its PDB file if it exists.
        /// </summary>
        /// <param name="pack">The mod data being associated with the assembly.</param>
        /// <param name="path">The path to the assembly being loaded.</param>
        /// <returns>The assembly that was loaded from the <see cref="path"/>.</returns>
        [CanBeNull]
        public static Assembly LoadAssembly(ModContentPack pack, [NotNull] string path)
        {
            string pdbPath = Path.ChangeExtension(path, ".pdb");
            byte[] content = File.ReadAllBytes(path);
            byte[] pdbContent = File.Exists(pdbPath) ? File.ReadAllBytes(pdbPath) : Array.Empty<byte>();

            Assembly assembly;

            try
            {
                assembly = AppDomain.CurrentDomain.Load(content, pdbContent);
            }
            catch (Exception e)
            {
                Logger.Error($"Could not load assembly @ {path}", e);

                return null;
            }

            AssemblyIsUsableMethod.Invoke(pack.assemblies, new object[] { assembly });
            pack.assemblies.loadedAssemblies.Add(assembly);

            InstantiateModClasses(pack, assembly);

            try
            {
                RunStaticConstructors(assembly);
            }
            catch (Exception e)
            {
                Logger.Error($"Encountered an error while running static constructors for {pack.Name}'s assembly @ {path}", e.InnerException ?? e);
            }

            return assembly;
        }

        /// <summary>
        ///     Runs the static constructors of all types annotated with
        ///     <see cref="StaticConstructorOnStartup"/> in an assembly.
        /// </summary>
        /// <param name="assembly">
        ///     An assembly with types annotated with
        ///     <see cref="StaticConstructorOnStartup"/>.
        /// </param>
        public static void RunStaticConstructors([NotNull] Assembly assembly)
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
                stringBuilder.AppendLine("ReflectionTypeLoadException getting types in assembly " + assembly.GetName().Name + ": " + e);
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

                Logger.Error(stringBuilder.ToString());
            }
        }

        /// <summary>
        ///     Loads a native assembly.
        /// </summary>
        /// <param name="path">The file path of the assembly.</param>
        /// <returns>A pointer to the assembly that was loaded.</returns>
        public static IntPtr LoadNativeAssembly(string path)
        {
            switch (UnityData.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                    return PInvokesWindows.LoadLibrary(path);
                case RuntimePlatform.OSXPlayer:
                    return PInvokesOsx.dlopen(path, (int)PosixDlOpen.Lazy);
                case RuntimePlatform.LinuxPlayer:
                    return PInvokesLinux.dlopen(path, (int)PosixDlOpen.Lazy);
                default:
                    Logger.Error($"Attempted to load native assembly @ {path} on unsupported platform '{UnityData.platform.ToString()}'");

                    return IntPtr.Zero;
            }
        }

        /// <summary>
        ///     Instantiates <see cref="Mod"/> classes, and adds them to an
        ///     internal field in RimWorld's <see cref="LoadedModManager"/>
        ///     class.
        /// </summary>
        /// <param name="mod">The mod the assembly belongs to.</param>
        /// <param name="assembly">
        ///     The assembly being indexed for
        ///     <see cref="Mod"/> classes.
        /// </param>
        public static void InstantiateModClasses(ModContentPack mod, [NotNull] Assembly assembly)
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
                    Logger.Error($"Could not instantiate {type.FullDescription()}", e);
                }
            }
        }
    }
}
