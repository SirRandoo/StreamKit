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

using JetBrains.Annotations;
using Verse;

namespace StreamKit.UX;

/// <summary>
///     Contains the various translation strings used by the library.
/// </summary>
[PublicAPI]
[StaticConstructorOnStartup]
public static class UxLocale
{
    /// <summary>
    ///     Contains the translation string for an "invalid url" error.
    /// </summary>
    [Translation("StreamKit.UX.InvalidUrl")]
    public static readonly string InvalidUrl = null!;

    /// <summary>
    ///     Contains the translation string for an "experimental content" notice.
    /// </summary>
    [Translation("StreamKit.UX.ExperimentalNotice")]
    public static readonly string ExperimentalNotice = null!;

    /// <summary>
    ///     Contains the translation string for an "invalid url" error, but colored redish-pinkish.
    /// </summary>
    [Translation("StreamKit.UX.InvalidUrl", UxColors.RedishPinkHex)]
    public static readonly string InvalidUrlColored = null!;

    /// <summary>
    ///     Contains the translation string for an "experimental content" notice, but colored
    ///     redish-pinkish.
    /// </summary>
    [Translation("StreamKit.UX.ExperimentalNotice", UxColors.RedishPinkHex)]
    public static readonly string ExperimentalNoticeColored = null!;

    [Translation("StreamKit.ModDependsOn")] public static readonly string ModDependsOn = null!;
    [Translation("StreamKit.ModDependsOn", UxColors.RedishPinkHex)] public static readonly string ModDependsOnColored = null!;

    static UxLocale()
    {
        TranslationManager.Register(typeof(UxLocale));
    }
}
