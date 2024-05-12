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
using System.Linq;
using System.Reflection;
using HarmonyLib;
using NLog;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api;

// FIXME: Currently most registries are empty synchronised registries.
// FIXME: Most registrants currently use an interface without any implementations of said interface.
// TODO: Registries should be initialized at start up, preferably in a way that doesn't block the main thread for extended periods of time.

public static class Registries
{
    public static readonly IRegistry<IProduct> ItemRegistry = new SynchronisedRegistry<IProduct>();
    public static readonly IRegistry<IProduct> TraitRegistry = new SynchronisedRegistry<IProduct>();
    public static readonly IRegistry<IProduct> PawnRegistry = new SynchronisedRegistry<IProduct>();
    public static readonly IRegistry<IProduct> EventRegistry = new SynchronisedRegistry<IProduct>();

    public static readonly IRegistry<IPlatform> PlatformRegistry = new MutableRegistry<IPlatform>(DomainIndexer.IndexFor<IPlatform>());
    public static readonly IRegistry<IComponent> ComponentRegistry = new MutableRegistry<IComponent>(DomainIndexer.IndexFor<IComponent>());
}

internal static class DomainIndexer
{
    private const string CreateInstanceMethodName = "CreateInstance";
    private const BindingFlags CreateInstanceBindingFlags = BindingFlags.Static | BindingFlags.Public;

    private static readonly Logger Logger = KitLogManager.GetLogger("StreamKit.DomainIndexer");

    internal static IList<T> IndexFor<T>() where T : class
    {
        List<T> results = AppDomain.CurrentDomain.GetAssemblies()
           .AsParallel()
           .SelectMany((assembly, _) => assembly.GetTypes())
           .Where(IsValidType<T>)
           .Select(CreateInstance<T>)
           .Where(c => c != null)
           .ToList()!;

        Logger.Warn("Found {Total} instances of {Type} in the current domain", results.Count, typeof(T).FullDescription());

        return results;
    }

    private static bool IsValidType<T>(Type type)
    {
        if (type.IsInterface)
        {
            return false;
        }

        if (type.IsAbstract)
        {
            return false;
        }

        if (type.Namespace == null || type.Namespace.StartsWith("System", StringComparison.Ordinal) || type.Namespace.StartsWith("Unity", StringComparison.Ordinal)
            || type.Namespace.StartsWith("Verse", StringComparison.Ordinal) || type.Namespace.StartsWith("RimWorld", StringComparison.Ordinal))
        {
            return false;
        }

        if (!typeof(T).IsAssignableFrom(type))
        {
            return false;
        }

        if (type.GetMethod(CreateInstanceMethodName, CreateInstanceBindingFlags) != null)
        {
            return true;
        }

        foreach (ConstructorInfo constructor in type.GetConstructors())
        {
            if (constructor.GetParameters().Any(parameter => !parameter.HasDefaultValue))
            {
                return false;
            }
        }

        return true;
    }

    private static T? CreateInstance<T>(Type type) where T : class
    {
        MethodInfo? method = type.GetMethod(CreateInstanceMethodName, CreateInstanceBindingFlags);

        if (method == null)
        {
            try
            {
                return Activator.CreateInstance(type) as T;
            }
            catch (Exception e)
            {
                Logger.Warn(e, "Could not create an instance of {Type}", type.FullDescription());

                return null;
            }
        }

        try
        {
            return method.Invoke(null, []) as T;
        }
        catch (Exception e)
        {
            Logger.Warn(e, "Could not constructor type '{Type}' from method '{MethodName}'", type.FullDescription(), CreateInstanceMethodName);

            return null;
        }
    }
}
