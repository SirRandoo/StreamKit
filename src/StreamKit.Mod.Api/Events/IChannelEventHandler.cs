using System.Threading.Tasks;
using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a special class for handling events emitted through the mod's event system.
/// </summary>
/// <typeparam name="TEvent">The type of event being handled by the handler.</typeparam>
[PublicAPI]
public interface IChannelEventHandler<in TEvent> : IIdentifiable where TEvent : PlatformEvent
{
    /// <summary>
    ///     The execution priority of the event handler. A higher value indicates that the handler should
    ///     be executed before handlers with a lower event handler.
    /// </summary>
    int Priority { get; }

    /// <summary>
    ///     Invoked when a new event needs to be handled by the event system handlers.
    /// </summary>
    /// <param name="event">The event being handled by the event handler.</param>
    /// <returns>
    ///     Whether the event should be passed along to event handlers after the currently executing
    ///     one.
    /// </returns>
    Task<ChannelEventResponse> HandleEvent(TEvent @event);
}
