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
using ConcurrentCollections;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Api;

/// <summary>
///     Represents a set of internal settings that may be set at runtime to change certain systems
///     within the mod.
/// </summary>
public static class RuntimeConfig
{
    private static readonly ConcurrentHashSet<string> Flags = [];

    /// <summary>
    ///     Returns all flags currently set within the config.
    /// </summary>
    public static IReadOnlyList<string> AllFlags => [..Flags];

    /// <summary>
    ///     Raised when a flag is (un)set.
    /// </summary>
    public static event EventHandler<RuntimeFlagChangedEventArgs>? RuntimeFlagChanged;

    /// <summary>
    ///     Sets a flag within the config.
    /// </summary>
    /// <param name="flag">The flag to set.</param>
    /// <returns>Whether the flag was added to the config.</returns>
    public static bool SetFlag(string flag)
    {
        bool added = Flags.Add(flag);

        if (added)
        {
            OnRuntimeFlagChanged(new RuntimeFlagChangedEventArgs { Flag = flag, Active = true });
        }

        return added;
    }

    /// <summary>
    ///     Returns whether a flag is currently set within the config.
    /// </summary>
    /// <param name="flag">The flag to check for.</param>
    public static bool HasFlag(string flag) => Flags.Contains(flag);

    /// <summary>
    ///     Unsets a flag within the config.
    /// </summary>
    /// <param name="flag">The flag to unset.</param>
    /// <returns>Whether the flag was removed from the config.</returns>
    public static bool UnsetFlag(string flag)
    {
        bool removed = Flags.TryRemove(flag);

        if (removed)
        {
            OnRuntimeFlagChanged(new RuntimeFlagChangedEventArgs { Flag = flag, Active = false });
        }

        return removed;
    }

    /// <summary>
    ///     Toggles a flag within the config.
    /// </summary>
    /// <param name="flag">The flag to toggle.</param>
    /// <returns>Whether the flag was toggled.</returns>
    public static bool ToggleFlag(string flag)
    {
        bool changed = Flags.Contains(flag) ? Flags.TryRemove(flag) : Flags.Add(flag);

        if (changed)
        {
            OnRuntimeFlagChanged(new RuntimeFlagChangedEventArgs { Flag = flag, Active = changed });
        }

        return changed;
    }

    private static void OnRuntimeFlagChanged(RuntimeFlagChangedEventArgs e)
    {
        RuntimeFlagChanged?.Invoke(null, e);
    }

    /// <summary>
    ///     A container class that houses all flags that are supported by the mod.
    /// </summary>
    public static class RuntimeFlags
    {
        [Experimental]
        [Description("Whether to disable Harmony's, the RimWorld mod's, stacktrace caching.")]
        [Description("This flag should be active if you're actively experiencing a problem, since Harmony may remove important information from errors.")]
        public const string DisableHarmonyStacktraceCaching = "dev.harmony.exception.cache.disable";

        [Experimental]
        [Description("Whether to disable Harmony, the RimWorld mod's, stacktrace enhancing.")]
        [Description("If this flag isn't active, Harmony will adjust errors with patch information.")]
        [Description("This flag should be active if you're actively experiencing a problem, since Harmony may remove important information from errors.")]
        public const string DisableHarmonyStacktraceEnhancing = "dev.harmony.exception.prettifier.disable";
    }
}

public class RuntimeFlagChangedEventArgs : EventArgs
{
    public bool Active { get; init; }
    public required string Flag { get; init; }
}
