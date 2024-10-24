namespace StreamKit.Mod.Api.Events;

/// <summary>
///     The types of responses <see cref="IChannelEventHandler{TEvent}" /> can return that affect how
///     the mod's event system handles subsequent handler execution.
/// </summary>
public enum ChannelEventResponse
{
    /// <summary>
    ///     Indicates that the event system should continue executing event handlers.
    /// </summary>
    Continue,

    /// <summary>
    ///     Indicates that the event system should abort executing subsequent event handlers.
    /// </summary>
    Abort
}
