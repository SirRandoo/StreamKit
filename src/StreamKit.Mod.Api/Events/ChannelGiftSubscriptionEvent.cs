using JetBrains.Annotations;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a user being gifted a subscription to the channel.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/param[@name='Platform' or 'User']" />
/// <param name="Recipient">The user who received the subscription.</param>
/// <param name="Tier">The optional subscription tier that was gifted to the recipient.</param>
[PublicAPI]
public sealed record ChannelGiftSubscriptionEvent(IPlatform Platform, IUser User, IUser Recipient, string? Tier = null) : PlatformEvent(Platform, User);