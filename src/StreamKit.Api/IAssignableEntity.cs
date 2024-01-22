using StreamKit.Data.Abstractions;
using UnityEngine;

namespace StreamKit.Mod;

public interface IAssignableEntity
{
    Texture2D PreviewImage { get; }
    IViewerData AssignedViewer { get; set; }
    AssignableEntityType Type { get; init; }

    void Recache();
}