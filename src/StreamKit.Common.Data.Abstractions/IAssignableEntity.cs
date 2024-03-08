using StreamKit.Mod.Api;

namespace StreamKit.Common.Data.Abstractions;

// TODO: This class uses an enum to describe assignable entity types. Maybe a generic of IIdentifiable should be considered.

// TODO: If a generic of IIdentifiable is used, an "entity wrapper" will have to be used to support in-game entities.
// TODO: This class stores a reference to a viewer's data. This should be simplified into a mini type that only stores the viewer's id and the platform they're on.
// TODO: Alternatively, a generic of Pawn could be used instead of IIdentifiable since this only pawns can ever be assigned.

// TODO: Re-evaluate this interface when the flat buffer contracts are fleshed out.

/// <summary>
///     Represents an entity that can be assigned to a viewer.
/// </summary>
public interface IAssignableEntity
{
    IUser AssignedUser { get; init; }
    byte[] PreviewImage { get; init; }
    AssignableEntityType Type { get; init; }
}
