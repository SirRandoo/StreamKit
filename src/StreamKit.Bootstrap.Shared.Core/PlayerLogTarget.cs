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

using System.Threading;
using NLog;
using NLog.Targets;
using Verse;

namespace StreamKit.Bootstrap.Shared.Core;

[Target(TargetName)]
[StaticConstructorOnStartup]
internal class PlayerLogTarget : TargetWithContext
{
    private const string TargetName = "PlayerLog";
    private static readonly SynchronizationContext CurrentContext = SynchronizationContext.Current;

    public PlayerLogTarget()
    {
        Name = TargetName;
    }

    protected override void Write(LogEventInfo logEvent)
    {
        string logMessage = RenderLogEvent(Layout, logEvent);

        if (logEvent.Level == LogLevel.Warn)
        {
            CurrentContext.Post(LogRedMessage, logMessage);
        }
        else if (logEvent.Level == LogLevel.Fatal || logEvent.Level == LogLevel.Error)
        {
            CurrentContext.Post(LogSeriousRedMessage, logMessage);
        }
        else
        {
            CurrentContext.Post(LogMessage, logMessage);
        }
    }

    private static void LogMessage(object message)
    {
        Log.Message((string)message);
    }

    private static void LogRedMessage(object message)
    {
        Log.Warning((string)message);
    }

    private static void LogSeriousRedMessage(object message)
    {
        Log.Error((string)message);
    }
}
