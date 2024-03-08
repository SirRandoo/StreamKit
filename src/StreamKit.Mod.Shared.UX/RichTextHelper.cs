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

using System.Linq;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Shared.UX;

// TODO: Revisit this for any potential optimizations.

/// <summary>
///     A class for removing XHTML tags from text.
/// </summary>
public static class RichTextHelper
{
    private static readonly string[] SupportedTags = ["b", "i", "size", "color", "material", "quad"];

    private static bool IsRichText(this string input)
    {
        string lowered = input.ToLowerInvariant();

        for (var index = 0; index < SupportedTags.Length; index++)
        {
            string tag = SupportedTags[index];

            if (lowered.Contains($"<{tag}") && lowered.Contains($"</{tag}>"))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Removes XHTML tags from a given string.
    /// </summary>
    /// <param name="input">The string to remove tags from</param>
    /// <returns>A potentially sanitized string</returns>
    public static string StripTags(this string input)
    {
        string container = input;
        var i = 0;
        int expectedTags = Mathf.Min(input.Count(ch => ch.Equals('<')), input.Count(ch => ch.Equals('>')));

        while (IsRichText(container))
        {
            var nameEnd = false;
            var inTag = false;
            var tag = "";

            string tagContent = container.Aggregate("", (current, c) => ProcessCharacter(c, current, ref inTag, ref nameEnd, ref tag));

            if (!tagContent.NullOrEmpty())
            {
                container = container.ReplaceFirst($"<{tagContent}>", "");
                container = container.ReplaceFirst($"</{tag}>", "");
            }

            i++;

            // While this may not catch everything, this should help prevent infinite loops. For
            // the types of content this method will be used for, any strings that contain more
            // than 10 tags is questionable.
            if ((container.Contains("<") || container.Contains(">")) && i > expectedTags)
            {
                return container;
            }
        }

        return container;
    }

    private static string ProcessCharacter(char c, string tagContent, ref bool inTag, ref bool nameEnd, ref string tag)
    {
        switch (c)
        {
            case '<' when tagContent == "":
                inTag = true;

                break;
            case '=' when inTag:
                nameEnd = true;
                tagContent += c.ToString();

                break;
            case '>':
                inTag = false;

                break;
            default:
            {
                if (inTag)
                {
                    tagContent += c.ToString();

                    if (!nameEnd)
                    {
                        tag += c.ToString();
                    }
                }

                break;
            }
        }

        return tagContent;
    }

    /// <summary>
    ///     Surrounds the given text with the specified rich text tag.
    /// </summary>
    /// <param name="s">The text to surround</param>
    /// <param name="tag">The rich text tag to surround the text with</param>
    /// <returns>The tagged string</returns>
    public static string Tagged(this string s, string tag) => $"<{tag}>{s}</{tag}>";

    /// <summary>
    ///     Surrounds the given text with the specified rich text color tag.
    /// </summary>
    /// <param name="s">The text to surround</param>
    /// <param name="hex">The specified color's hex code</param>
    /// <returns>The color tagged text</returns>
    public static string ColorTagged(this string s, string hex)
    {
        if (!hex.StartsWith("#"))
        {
            hex = $"#{hex}";
        }

        return $"""<color="{hex}">{s}</color>""";
    }

    /// <summary>
    ///     Surrounds the given text with the specified rich text tag.
    /// </summary>
    /// <param name="s">The text to surround</param>
    /// <param name="color">The specified color</param>
    /// <returns>The color tagged text</returns>
    public static string ColorTagged(this string s, Color color) => ColorTagged(s, ColorUtility.ToHtmlStringRGB(color));
}
