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
using System.Reflection;
using System.Text;
using HarmonyLib;
using SirRandoo.CommonLib.Entities;
using SirRandoo.CommonLib.Interfaces;
using StreamKit.Api.Attributes;
using Verse;
using DescriptionAttribute = StreamKit.Api.Attributes.DescriptionAttribute;

namespace StreamKit.Api;

public static class ModSettingFactory
{
    private static readonly IRimLogger Logger = new RimLogger("StreamKit.Factories.ModSettings");

    public static IReadOnlyList<ModSetting> FromInstance<T>(T instance) where T : class, IComponentSettings
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        var settings = new List<ModSetting>();

        for (var i = 0; i < properties.Length; i++)
        {
            PropertyInfo property = properties[i];

            if (property.HasAttribute<InternalSettingAttribute>())
            {
                continue;
            }

            settings.Add(FromProperty(instance, property));
        }

        settings.Sort((setting, modSetting) => setting.Order.CompareTo(modSetting.Order));

        return settings;
    }

    public static ModSetting FromProperty<T>(T instance, PropertyInfo property) where T : class, IComponentSettings
    {
        string? label = GetSettingLabel(property);
        string? description = GetSettingDescription(property);

        int order = property.TryGetAttribute(out OrderAttribute orderAttribute) ? orderAttribute.Order : 0;
        ITypeDrawer drawer = GetTypeDrawer(property, instance);

        return new ModSetting(label, drawer, order, description);
    }

    private static string? GetSettingDescription(MemberInfo property)
    {
        if (!property.HasAttribute<DescriptionAttribute>())
        {
            return null;
        }

        var builder = new StringBuilder();

        foreach (DescriptionAttribute attribute in property.GetCustomAttributes<DescriptionAttribute>())
        {
            if (!attribute.Inline && builder.Length > 0)
            {
                builder.Append("\n");
            }

            builder.Append(attribute.IsKey ? attribute.Text.TranslateSimple() : attribute.Text);
            builder.Append(" ");
        }

        return builder.Remove(builder.Length - 1, 1).ToString();
    }

    private static string GetSettingLabel(MemberInfo property)
    {
        if (!property.TryGetAttribute(out LabelAttribute labelAttribute))
        {
            return property.Name;
        }

        return labelAttribute.IsKey ? labelAttribute.Text.TranslateSimple() : labelAttribute.Text;
    }

    private static ITypeDrawer GetTypeDrawer<T>(PropertyInfo property, T instance) where T : class
    {
        object[] constructorParams;
        Type drawer = (property.TryGetAttribute(out DrawerAttribute drawerAttribute) ? drawerAttribute.Type : DrawerRegistry.GetDrawer(property.PropertyType));

        if (drawer.GetConstructor([typeof(Type)]) != null)
        {
            constructorParams = [property.PropertyType];
        }
        else if (drawer.GetConstructor([property.PropertyType]) != null)
        {
            constructorParams = [property.GetMethod.Invoke(instance, [])];
        }
        else
        {
            constructorParams = [];
        }

        if (Activator.CreateInstance(drawer, constructorParams) is not ITypeDrawer drawerInstance)
        {
            throw new InvalidOperationException("Attempted to create a setting drawer instance, but the specified type wasn't a settings drawer implementation.");
        }

        PropertyInfo? getterProperty = drawer.GetProperty("Getter");

        if (getterProperty != null)
        {
            getterProperty.SetValue(drawerInstance, () => property.GetMethod.Invoke(instance, []));
        }

        PropertyInfo? setterProperty = drawer.GetProperty("Setter");

        if (setterProperty != null)
        {
            setterProperty.SetValue(
                drawerInstance,
                (object value) =>
                {
                    property.SetMethod.Invoke(instance, [value]);
                }
            );
        }

        try
        {
            drawerInstance.Initialise();
        }
        catch (Exception e)
        {
            Logger.Error($"Could not initialise setting for {instance.GetType().FullDescription()}.{property.Name}");
        }

        return drawerInstance;
    }
}
