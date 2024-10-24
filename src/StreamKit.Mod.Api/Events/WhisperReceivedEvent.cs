using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a whisper being received by a user.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/params[@name='Platform' or 'User']" />
/// <param name="WhisperId">The id of the whisper that was received.</param>
/// <param name="Recipient">The user who received the whisper from the user.</param>
/// <param name="WhisperContent">The content of the whisper message.</param>
[PublicAPI]
public sealed record WhisperReceivedEvent(IPlatform Platform, IUser User, string WhisperId, IUser Recipient, string WhisperContent) : PlatformEvent(Platform, User);
