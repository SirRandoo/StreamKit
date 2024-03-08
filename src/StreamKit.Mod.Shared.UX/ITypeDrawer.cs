using System;
using UnityEngine;

namespace StreamKit.Mod.Shared.UX;

/// <summary>
///     Represents a specialized class for drawing a given type on screen using Unity's IMGUI system.
/// </summary>
public interface ITypeDrawer
{
    /// <summary>
    ///     Draws the given type on the screen.
    /// </summary>
    /// <param name="region">The region to draw the graphical representation of the type.</param>
    void Draw(ref Rect region);

    /// <summary>
    ///     Toggles the underlying value, assuming it has a value that can be toggled.
    /// </summary>
    void Toggle();

    /// <summary>
    ///     Called once after the drawer is constructed, and after the factory assigns all properties it's
    ///     tasked with assigning.
    /// </summary>
    void Initialise();
}


public interface ITypeDrawer<T> : ITypeDrawer
{
    Func<object> Getter { get; set; }
    Action<object?> Setter { get; set; }

    T? Value { get; set; }
}
