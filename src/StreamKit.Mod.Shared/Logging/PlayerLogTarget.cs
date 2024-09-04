using System.Threading;
using NLog;
using NLog.Targets;
using Verse;

namespace StreamKit.Mod.Shared.Logging;

[Target(TargetName)]
public class PlayerLogTarget : TargetWithContext
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

        if (logEvent.Level == LogLevel.Warn || logEvent.Level == LogLevel.Fatal || logEvent.Level == LogLevel.Error)
        {
            CurrentContext.Post(LogRedMessage, logMessage);
        }
        else
        {
            CurrentContext.Post(LogMessage, logMessage);
        }
    }

    private static void LogMessage(object logMessage)
    {
        Log.Message((string)logMessage);
    }

    private static void LogRedMessage(object logMessage)
    {
        Log.Warning((string)logMessage);
    }
}
