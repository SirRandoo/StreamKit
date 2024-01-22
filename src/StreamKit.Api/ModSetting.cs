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
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using GenAttribute = Verse.GenAttribute;
using Translator = Verse.Translator;

namespace StreamKit.Mod;

internal sealed record ModSetting(object Instance, string Label, string? Description, int Order, PropertyInfo Property, MethodInfo Getter, MethodInfo Setter)
{
    public Action<Rect, ModSetting>? FieldDrawer { get; set; }
    public string? Buffer { get; set; }
    public bool BufferValid { get; set; } = true;

    public static ModSetting CreateInstance(object instance, PropertyInfo property)
    {
        bool translatable = GenAttribute.HasAttribute<LocalizableAttribute>(property);
        string label = GenAttribute.TryGetAttribute(property, out DisplayNameAttribute nameAttribute) ? nameAttribute.DisplayName : property.Name;
        string? description = GenAttribute.TryGetAttribute(property, out DescriptionAttribute descriptionAttribute) ? descriptionAttribute.Description : null;

        if (translatable)
        {
            label = Translator.TranslateSimple(label);
        }

        return new ModSetting(instance, label, description, GetOrder(property.PropertyType), property, property.GetMethod, property.SetMethod)
        {
            Buffer = property.GetMethod.Invoke(instance, []).ToString()
        };
    }

    private static int GetOrder(Type type) => type == typeof(bool) ? 10 : 0;
}
