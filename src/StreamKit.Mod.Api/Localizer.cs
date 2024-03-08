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

namespace StreamKit.Mod.Api;

/// <summary>
///     A static class dedicated to translating text into other languages supported by the mod, and
///     RimWorld.
/// </summary>
public static class Localizer
{
    /// <summary>
    ///     Marks text as untranslated.
    /// </summary>
    /// <param name="text">The text that isn't translated.</param>
    /// <remarks>
    ///     This method is intended to be used by developers to mark strings as untranslated. This method
    ///     alone does nothing if a developer's workflow doesn't track obsolete methods.
    /// </remarks>
    [Obsolete("Callers should translate the string passed prior to release.")]
    public static string MarkNotTranslated(this string text) => text;
}
