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
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Verse;

namespace StreamKit.Bootstrap.Shared.Core;

public class KitLogManager
{
    private static readonly Lazy<KitLogManager> InternalInstance = new(CreateManager);
    private LogFactory _factory = null!;

    public static KitLogManager Instance => InternalInstance.Value;

    public Logger GetLogger(string name) => _factory.GetLogger(name);

    private static KitLogManager CreateManager()
    {
        var playerLogTarget = new PlayerLogTarget();
        var fileTarget = new FileTarget("StreamKit.File");
        var asyncWrapper = new AsyncTargetWrapper(fileTarget);

        playerLogTarget.Layout = "${time} [${level:uppercase=true}][Thread ${threadid}] (${logger}) :: ${message:withexception=true}";

        fileTarget.DeleteOldFileOnStartup = true;
        fileTarget.Layout = playerLogTarget.Layout;
        fileTarget.FileName = Path.Combine(GenFilePaths.SaveDataFolderPath, "StreamKit", "streamkit.log");

        var config = new LoggingConfiguration();
        config.AddTarget(asyncWrapper);
        config.AddTarget(playerLogTarget);
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, asyncWrapper);
        config.AddRule(LogLevel.Warn, LogLevel.Fatal, playerLogTarget);

#if DEBUG
        var consoleTarget = new ColoredConsoleTarget("StreamKit.Console");
        consoleTarget.Layout = playerLogTarget.Layout;

        var asyncConsoleTarget = new AsyncTargetWrapper(consoleTarget);

        config.AddTarget(asyncConsoleTarget);
        config.AddRule(LogLevel.Trace, LogLevel.Fatal, asyncConsoleTarget);
#endif

        return new KitLogManager { _factory = new LogFactory { Configuration = config } };
    }
}
