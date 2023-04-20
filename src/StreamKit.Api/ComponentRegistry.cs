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

namespace StreamKit.Api
{
    /// <summary>
    ///     A class for housing <see cref="IComponent"/>s that were
    ///     registered with the mod.
    ///     <br/>
    ///     <br/>
    ///     Registering a component with the mod allows said
    ///     component to be managed by the mod. Components that are
    ///     managed by the mod are asked to perform certain
    ///     lifecycle events, like loading settings. Managed components also
    ///     delegate their settings menu to the mod itself.
    /// </summary>
    public static class ComponentRegistry
    {
        private static readonly Registry<IComponent> Registry = new Registry<IComponent>();

        /// <summary>
        ///     Returns a list of <see cref="WeakReference{T}"/>s to all the
        ///     <see cref="IComponent"/>s registered within the registry.
        /// </summary>
        [NotNull]
        public static List<IComponent> AllComponents => Registry.AllRegistrants;

        /// <summary>
        ///     Returns a <see cref="IComponent"/> that was previously registered
        ///     by the component's id.
        /// </summary>
        /// <param name="id">The unique id of the <see cref="IComponent"/>.</param>
        /// <returns>
        ///     A <see cref="WeakReference{T}"/> to the <see cref="IComponent"/>
        ///     that was registered under the given id, or <see langword="null"/>
        ///     if there was no component registered with that id.
        /// </returns>
        [CanBeNull]
        public static IComponent Get([NotNull] string id) => Registry.Get(id);

        /// <summary>
        ///     Registers a <see cref="IComponent"/> within the registry.
        /// </summary>
        public static void Register([NotNull] IComponent component) => Registry.Register(component);

        /// <summary>
        ///     Unregisters a <see cref="IComponent"/> from the registry.
        /// </summary>
        /// <returns>Whether the <see cref="IComponent"/> was unregistered.</returns>
        public static bool Unregister(IComponent component) => Registry.Unregister(component);
    }
}
