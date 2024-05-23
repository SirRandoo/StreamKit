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
