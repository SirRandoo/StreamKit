namespace StreamKit.Mod.Api.Events;

/// <summary>
///     Represents a choice within a poll in a given channel.
/// </summary>
/// <param name="Id">The id of the choice within the poll.</param>
/// <param name="Votes">The number of votes the choice received within the poll.</param>
public sealed record ChannelPollChoice(string Id, int Votes);
