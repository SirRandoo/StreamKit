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

namespace StreamKit.UX;

/// <summary>
///     Used to mark fields as a container for a given translation string.
/// </summary>
/// <param name="key">
///     The translation string's key. This is typically the name of an XML element within a mod's
///     translation file, but may be broken in pieces should
///     <see cref="TranslationNamespaceAttribute" /> is also used.
/// </param>
/// <param name="color"></param>
[AttributeUsage(AttributeTargets.Field)]
public sealed class TranslationAttribute(string key, string? color = null) : Attribute
{
    /// <inheritdoc cref="TranslationAttribute" path="/param[@name='key']" />
    public string Key { get; } = key;

    /// <summary>
    ///     The optional color of the translation string.
    /// </summary>
    public string? Color { get; } = color;
}

/// <summary>
///     Used to mark classes, that contain one or more <see cref="TranslationAttribute" />d fields,
///     with a translation namespace. Translation namespaces are simply strings that are prefixed to
///     all <see cref="TranslationAttribute.Key" /> within the class.
/// </summary>
/// <param name="namespace">The namespace of translations within the class.</param>
/// <remarks>
///     This is typically used to reduce the amount of horizontal scrolling required to see class
///     fields that have a <see cref="TranslationAttribute" />.
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public sealed class TranslationNamespaceAttribute(string @namespace) : Attribute
{
    /// <inheritdoc cref="TranslationNamespaceAttribute" path="/param[@name='namespace']" />
    public string Namespace { get; } = @namespace;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class TranslationRecacheListenerAttribute : Attribute
{
}
