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
using System.Collections.Immutable;
using StreamKit.Api.UX.Drawers;

namespace StreamKit.Api;

public static class DrawerRegistry
{
    private static readonly ImmutableDictionary<Type, Type> Drawers = new Dictionary<Type, Type>
    {
        { typeof(bool), typeof(BoolTypeDrawer) },
        { typeof(float), typeof(FloatTypeDrawer) },
        { typeof(int), typeof(IntTypeDrawer) },
        { typeof(ShortRange), typeof(ShortRangeTypeDrawer) },
        { typeof(short), typeof(ShortTypeDrawer) },
        { typeof(string), typeof(StringTypeDrawer) },
        { typeof(TimeSpan), typeof(TimeSpanTypeDrawer) },
        { typeof(Uri), typeof(UriTypeDrawer) }
    }.ToImmutableDictionary();

    public static Type GetDrawer(Type type) => Drawers.TryGetValue(type, out Type? drawerType) ? drawerType : typeof(UnknownTypeDrawer);
}
