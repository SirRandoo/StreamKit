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
using System.Diagnostics;
using HarmonyLib;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Verse;

namespace StreamKit.Mod.Shared.Logging;

[StaticConstructorOnStartup]
public static class KitLogManager
{
    private const string Layout = "${time} [${level:uppercase=true}][Thread ${threadid}] (${logger}) :: ${message:withexception=true}";

    private static readonly Lazy<LogFactory> Manager = new(CreateManager);

    public static Logger GetLogger(string name) => Manager.Value.GetLogger(name);
    public static Logger GetLogger(Type type) => Manager.Value.GetLogger(type.FullDescription());
    public static Logger GetLogger<T>() where T : class => Manager.Value.GetLogger(typeof(T).FullDescription());

    private static LogFactory CreateManager()
    {
        var factory = new LogFactory();
        var config = new LoggingConfiguration();

        var playerLogTarget = new PlayerLogTarget();
        playerLogTarget.Layout = Layout;
        config.AddTarget(playerLogTarget);
        config.AddRule(LogLevel.Warn, LogLevel.Fatal, playerLogTarget);

        RegisterConsoleTarget(config);

        factory.Configuration = config;

        return factory;
    }

    [Conditional("DEBUG")]
    private static void RegisterConsoleTarget(LoggingConfiguration configuration)
    {
        if (!GenCommandLine.TryGetCommandLineArg("-logfile", out string? value) && string.Equals(value, "-", StringComparison.Ordinal))
        {
            return;
        }

        var consoleTarget = new ColoredConsoleTarget("StreamKit.Console");
        consoleTarget.Layout = Layout;

        var wrapper = new AsyncTargetWrapper(consoleTarget);

        configuration.AddTarget(wrapper);
        configuration.AddRule(LogLevel.Trace, LogLevel.Fatal, wrapper);
    }
}
