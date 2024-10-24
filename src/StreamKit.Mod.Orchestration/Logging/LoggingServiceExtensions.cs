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
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Verse;
using LogLevel = NLog.LogLevel;

namespace StreamKit.Mod.Orchestration.Logging;

internal static class LoggingServiceExtensions
{
    private const string LogMessageLayout = "${time} [${level:uppercase=true}][Thread ${threadid}] (${logger}) :: ${message:withexception=true}";

    internal static void ConfigureNLog(HostBuilderContext context, ILoggingBuilder builder)
    {
        LoggingConfiguration configuration = BuildNLogConfiguration();
        InjectConsoleTarget(configuration);

        builder.AddNLog(configuration);
    }

    private static LoggingConfiguration BuildNLogConfiguration()
    {
        var config = new LoggingConfiguration();
        var playerLogTarget = new PlayerLogTarget { Layout = LogMessageLayout };
        var asyncTargetWrapper = new AsyncTargetWrapper(playerLogTarget);

        config.AddTarget(asyncTargetWrapper);
        config.AddRule(LogLevel.Warn, LogLevel.Fatal, playerLogTarget);

        return config;
    }

    [Conditional("DEBUG")]
    private static void InjectConsoleTarget(LoggingConfiguration config)
    {
        if (!GenCommandLine.TryGetCommandLineArg("-logfile", out string? value) && !string.Equals(value, "-", StringComparison.Ordinal))
        {
            return;
        }

        var consoleTarget = new ColoredConsoleTarget { Layout = LogMessageLayout };
        var consoleTargetWrapper = new BufferingTargetWrapper(consoleTarget);

        config.AddTarget(consoleTargetWrapper);
        config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTargetWrapper);
    }
}
