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
using JetBrains.Annotations;
using StreamKit.Api;

namespace StreamKit.Platform.Abstractions
{
    /// <summary>
    ///     A class for housing <see cref="IPlatform"/>s that were registered
    ///     with the mod.
    ///     <br/>
    ///     <br/>
    ///     Registering a <see cref="IPlatform"/> with the mod allows said
    ///     platform to be managed by the end-user. Platforms that are
    ///     managed by the mod are asked to perform certain lifecycle events,
    ///     like connecting on startup. Unlike components, platforms are
    ///     required to implement a <see cref="IPlatformSettings"/>
    ///     derivative that houses their platform specific settings, which
    ///     are then displayed to the user in a specific section of the
    ///     StreamKit's settings menu.
    /// </summary>
    public static class PlatformRegistry
    {
        private static readonly Registry<IPlatform> Registry = new Registry<IPlatform>();

        /// <summary>
        ///     Returns a list of <see cref="WeakReference{T}"/> for all the
        ///     <see cref="IPlatform"/>s registered within the registry.
        /// </summary>
        [NotNull]
        public static List<IPlatform> AllComponents => Registry.AllRegistrants;

        /// <summary>
        ///     Returns a <see cref="IPlatform"/> that was previously registered
        ///     by the platform's id.
        /// </summary>
        /// <param name="id">The unique id of the <see cref="IPlatform"/>.</param>
        /// <returns>
        ///     A <see cref="WeakReference{T}"/> to the <see cref="IPlatform"/>
        ///     that was registered under the given id, or <see langword="null"/>
        ///     if there was no platform associated with that id.
        /// </returns>
        [CanBeNull]
        public static IPlatform Get([NotNull] string id) => Registry.Get(id);

        /// <summary>
        ///     Registers a <see cref="IPlatform"/> within the registry.
        /// </summary>
        public static void Register([NotNull] IPlatform platform) => Registry.Register(platform);

        /// <summary>
        ///     Unregisters a <see cref="IPlatform"/> from the registry.
        /// </summary>
        /// <returns>Whether the <see cref="IPlatform"/> was unregistered.</returns>
        public static bool Unregister(IPlatform platform) => Registry.Unregister(platform);
    }
}
