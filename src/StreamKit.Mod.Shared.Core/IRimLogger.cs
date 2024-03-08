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
using System.Reflection;
using System.Threading;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Shared.Core;

// TODO: This interface, and it's implementations, exist to suppress errors, but should be replaced with a separate system that doesn't use RimWorld's log file.
// TODO: Contrary to what the class's name suggests, RimThreadedLogger only "quirk" is that it logs messages on the game's main thread if a method is used on a separate thread.

/// <summary>
///     An interface for outlining the a logger for RimWorld.
/// </summary>
public interface IRimLogger
{
    /// <summary>
    ///     Formats a log message.
    /// </summary>
    /// <param name="message">The message to format</param>
    /// <returns>The formatted message</returns>
    string FormatMessage(string message);

    /// <summary>
    ///     Formats a log message.
    /// </summary>
    /// <param name="level">The log level of the message</param>
    /// <param name="message">The message to format</param>
    /// <returns>The formatted message</returns>
    string FormatMessage(string level, string message);

    /// <summary>
    ///     Formats a log message.
    /// </summary>
    /// <param name="level">The log level of the message</param>
    /// <param name="message">The message to format</param>
    /// <param name="color">The color code of the message</param>
    /// <returns>The formatted message</returns>
    string FormatMessage(string level, string message, string color);

    /// <summary>
    ///     Logs a message to RimWorld's log window.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Log(string message);

    /// <summary>
    ///     Logs an INFO level message to RimWorld's log window.
    /// </summary>
    /// <param name="message">The message to log</param>
    void Info(string message);

    /// <summary>
    ///     Logs a WARN level message to RimWorld's log window.
    /// </summary>
    /// <param name="message">The message to log</param>
    void Warn(string message);

    /// <summary>
    ///     Logs an ERROR level message to RimWorld's log window.
    /// </summary>
    /// <param name="message">The message to log</param>
    void Error(string message);

    /// <summary>
    ///     Logs an ERROR level message to RimWorld's log window.
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="exception">The exception to log</param>
    void Error(string message, Exception exception);

    /// <summary>
    ///     Logs a DEBUG level message to RimWorld's log window.
    /// </summary>
    /// <param name="message">The message to log</param>
    void Debug(string message);
}

public class RimLogger(string name) : IRimLogger
{
    private bool _debugChecked;
    private bool _debugEnabled;

    /// <inheritdoc />
    public string FormatMessage(string message) => $"{name} :: {message}";

    /// <inheritdoc />
    public string FormatMessage(string level, string message) => $"{level.ToUpperInvariant()} {name} :: {message}";

    /// <inheritdoc />
    public string FormatMessage(string level, string message, string color) => $"""<color="#{color.TrimStart('#')}">{FormatMessage(level, message)}</color>""";

    /// <inheritdoc />
    public virtual void Log(string message)
    {
        LogInternal(FormatMessage(message));
    }

    /// <inheritdoc />
    public virtual void Info(string message)
    {
        LogInternal(FormatMessage("INFO", message));
    }

    /// <inheritdoc />
    public virtual void Warn(string message)
    {
        LogInternal(FormatMessage("WARN", message, "#FF6B00"));
    }

    /// <inheritdoc />
    public virtual void Error(string message)
    {
        LogInternal(FormatMessage("ERR", message, "#FF768CE"));
        Verse.Log.TryOpenLogWindow();
    }

    /// <inheritdoc />
    public virtual void Error(string message, Exception exception)
    {
        Error($"{message} :: {exception.GetType().Name}({exception.Message})\n\n{exception.ToStringSafe()}");
    }

    /// <inheritdoc />
    public virtual void Debug(string message)
    {
        if (!_debugChecked)
        {
            _debugEnabled = Assembly.GetCallingAssembly().GetCustomAttribute<DebuggableAttribute>()?.DebuggingFlags
                == DebuggableAttribute.DebuggingModes.DisableOptimizations;

            _debugChecked = true;
        }

        if (_debugEnabled)
        {
            LogInternal(FormatMessage("DEBUG", message, ColorUtility.ToHtmlStringRGB(ColorLibrary.LightPink)));
        }
    }

    protected virtual void LogInternal(string message)
    {
        Verse.Log.Message(message);
    }
}

[StaticConstructorOnStartup]
public class RimThreadedLogger(string name) : RimLogger(name)
{
    private static readonly SynchronizationContext Context = SynchronizationContext.Current;

    /// <inheritdoc />
    public override void Log(string message)
    {
        if (UnityData.IsInMainThread)
        {
            base.Log(message);
        }
        else
        {
            Context.Post(o => base.Log(message), null);
        }
    }

    /// <inheritdoc />
    public override void Error(string message)
    {
        if (UnityData.IsInMainThread)
        {
            base.Error(message);
        }
        else
        {
            Context.Post(o => base.Error(message), null);
        }
    }
}
