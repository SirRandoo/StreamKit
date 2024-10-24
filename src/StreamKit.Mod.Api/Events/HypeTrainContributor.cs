using JetBrains.Annotations;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a contributor within a channel's hype train.
/// </summary>
/// <inheritdoc cref="PlatformEvent" path="/param[@name='Platform' or 'User']" />
/// <param name="Total">The total number of points the user contributed to the hype train.</param>
/// <param name="ContributionType">The contribution method used.</param>
[PublicAPI]
public sealed record HypeTrainContributor(IPlatform Platform, IUser User, int Total, HypeTrainContributionType ContributionType);
