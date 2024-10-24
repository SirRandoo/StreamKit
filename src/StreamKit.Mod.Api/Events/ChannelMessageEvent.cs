using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a message that was sent in the channel.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/param[@name='Platform' or 'User']" />
/// <param name="MessageId">The id of the message.</param>
/// <param name="MessageContent">The raw content of the message.</param>
[PublicAPI]
public sealed record ChannelMessageEvent(IPlatform Platform, IUser User, string MessageId, string MessageContent) : PlatformEvent(Platform, User);
