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
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace StreamKit.UX;

/// <summary>
///     Manages any registered translation fields. This class is partially responsible for
/// </summary>
[PublicAPI]
public static class TranslationManager
{
    private static readonly List<TranslationIndex> TranslationIndices = [];
    private static readonly Dictionary<FieldInfo, TranslationIndex> TranslationIndicesKeyed = [];
    private static readonly List<TranslationListener> TranslationListeners = [];
    private static readonly Dictionary<MethodInfo, TranslationListener> TranslationListenersKeyed = [];

    /// <summary>
    ///     Registers any fields marked with <see cref="TranslationAttribute" /> found within the type.
    /// </summary>
    /// <param name="type">The type to index for translation fields.</param>
    public static void Register(Type type)
    {
        var @namespace = type.GetCustomAttribute<TranslationNamespaceAttribute>();

        RegisterFields(type, @namespace);
        RegisterListeners(type);
    }

    private static void RegisterFields(Type type, TranslationNamespaceAttribute @namespace)
    {
        foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public))
        {
            var attribute = field.GetCustomAttribute<TranslationAttribute>();

            if (attribute == null)
            {
                continue;
            }

            AccessTools.FieldRef<string> reference = AccessTools.StaticFieldRefAccess<string>(field);
            string fullKey = @namespace != null ? $"{@namespace.Namespace}.{attribute.Key}" : attribute.Key;

            var translationIndex = new TranslationIndex(field, reference, fullKey, attribute.Color);

            TranslationIndices.Add(translationIndex);
            TranslationIndicesKeyed.Add(field, translationIndex);

            if (string.IsNullOrEmpty(reference()))
            {
                RecacheTranslation(translationIndex);
            }
        }
    }

    private static void RegisterListeners(Type type)
    {
        foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public))
        {
            var attribute = method.GetCustomAttribute<TranslationRecacheListenerAttribute>();

            if (attribute == null)
            {
                continue;
            }

            var @delegate = AccessTools.MethodDelegate<ListenerDelegate>(method);
            var listener = new TranslationListener(method, @delegate);

            TranslationListeners.Add(listener);
            TranslationListenersKeyed.Add(method, listener);

            @delegate();
        }
    }

    /// <summary>
    ///     Removes any previously registered fields within the given type.
    /// </summary>
    /// <param name="type">The type to index for translation fields.</param>
    public static void Unregister(Type type)
    {
        for (int index = TranslationIndices.Count - 1; index >= 0; index--)
        {
            TranslationIndex field = TranslationIndices[index];

            if (field.Field.DeclaringType != type)
            {
                continue;
            }

            TranslationIndices.Remove(field);
            TranslationIndicesKeyed.Remove(field.Field);
        }

        for (var index = 0; index < TranslationListeners.Count; index++)
        {
            TranslationListener listener = TranslationListeners[index];

            if (listener.Method.DeclaringType != type)
            {
                continue;
            }

            TranslationListeners.Remove(listener);
            TranslationListenersKeyed.Remove(listener.Method);
        }
    }

    private static void RecacheTranslation(TranslationIndex index)
    {
        string key = index.Key;

        if (key.CanTranslate())
        {
            key = key.TranslateSimple();
        }

        if (!string.IsNullOrEmpty(index.Color))
        {
            key = index.Color!.StartsWith("#") ? $"""<color="{index.Color}">{key}</color>""" : $"""<color="#{index.Color}">{key}</color>""";
        }

        ref string indexRef = ref index.Ref();
        indexRef = key;
    }

    [PublicAPI]
    [HarmonyPatch]
    private static class SelectLanguagePatch
    {
        private static readonly MethodBase SelectLanguageMethod = AccessTools.Method(typeof(LanguageDatabase), nameof(LanguageDatabase.SelectLanguage));

        private static IEnumerable<MethodBase> TargetMethods()
        {
            yield return SelectLanguageMethod;
        }

        private static void Postfix()
        {
            RecacheTranslations();
        }

        private static void RecacheTranslations()
        {
            for (var i = 0; i < TranslationIndices.Count; i++)
            {
                RecacheTranslation(TranslationIndices[i]);
            }

            for (var i = 0; i < TranslationListeners.Count; i++)
            {
                TranslationListeners[i].Delegate();
            }
        }
    }

    private delegate Delegate ListenerDelegate();
    private sealed record TranslationIndex(FieldInfo Field, AccessTools.FieldRef<string> Ref, string Key, string? Color);
    private sealed record TranslationListener(MethodInfo Method, ListenerDelegate Delegate);
}

[UsedImplicitly]
[StaticConstructorOnStartup]
internal static class Patcher
{
    private static readonly Harmony HarmonyInstance = new("com.sirrandoo.ux");

    static Patcher()
    {
        HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
    }
}
