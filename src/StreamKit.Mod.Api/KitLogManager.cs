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

using System.Reflection;
using HarmonyLib;
using NLog;

namespace StreamKit.Mod.Api;

public static class KitLogManager
{
    private static readonly PropertyInfo InstancePropertyInfo = AccessTools.Property("StreamKit.Bootstrap.Shared.Core.KitLogManager:Instance");
    private static readonly MethodInfo GetLoggerMethodInfo = AccessTools.Method("StreamKit.Bootstrap.Shared.Core.KitLogManager:GetLogger");
    private static readonly AccessTools.FieldRef<object, LogFactory> FactoryFieldInfo = AccessTools.FieldRefAccess<LogFactory>("StreamKit.Bootstrap.Shared.Core.KitLogManager:_factory");

    public static LogFactory Factory
    {
        get
        {
            object? instance = InstancePropertyInfo.GetMethod.Invoke(null, []);

            return FactoryFieldInfo(instance);
        }
    }

    public static Logger GetLogger(string name)
    {
        object? instance = InstancePropertyInfo.GetMethod.Invoke(null, []);

        return (Logger)GetLoggerMethodInfo.Invoke(instance, [name]);
    }
}
