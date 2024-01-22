using StreamKit.Data.Abstractions;
using UnityEngine;

namespace StreamKit.Api;

// TODO: This class uses an enum to describe assignable entity types. Maybe a generic of IIdentifiable should be considered.

// TODO: If a generic of IIdentifiable is used, an "entity wrapper" will have to be used to support in-game entities.
// TODO: This class stores a reference to a viewer's data. This should be simplified into a mini type that only stores the viewer's id and the platform they're on.
// TODO: Alternatively, a generic of Pawn could be used instead of IIdentifiable since this only pawns can ever be assigned.

public interface IAssignableEntity
{
    Texture2D PreviewImage { get; }
    IViewerData AssignedViewer { get; set; }
    AssignableEntityType Type { get; init; }

    void Recache();
}
