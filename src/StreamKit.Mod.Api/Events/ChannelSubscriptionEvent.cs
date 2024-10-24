using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a user subscribing to the channel.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/param[@name='Platform' or 'User']" />
/// <param name="Tier">The optional subscription tier the user subscribed at.</param>
[PublicAPI]
public sealed record ChannelSubscriptionEvent(IPlatform Platform, IUser User, string? Tier = null) : PlatformEvent(Platform, User);
