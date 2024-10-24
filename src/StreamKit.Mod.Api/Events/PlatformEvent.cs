using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents an event within a channel.
/// </summary>
/// <param name="Platform">The platform the channel belongs to.</param>
/// <param name="User">
///     The user who's the subject of interest in the event. This can be a viewer, the
///     channel owner, or another channel in some instances.
/// </param>
[PublicAPI]
public abstract record PlatformEvent(IPlatform Platform, IUser User);
