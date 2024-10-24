using NLog;
using NLog.Targets;
using Verse;

namespace StreamKit.Mod.Orchestration.Logging;

[Target(TargetName)]
internal class PlayerLogTarget : TargetWithContext
{
    private const string TargetName = "PlayerLog";

    protected override void Write(LogEventInfo logEvent)
    {
        string logMessage = RenderLogEvent(Layout, logEvent);

        if (logEvent.Level == LogLevel.Warn || logEvent.Level == LogLevel.Fatal || logEvent.Level == LogLevel.Error)
        {
            Orchestration.SynchronizationContext.Post(LogRedMessage, logMessage);
        }
        else
        {
            Orchestration.SynchronizationContext.Post(LogMessage, logMessage);
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
