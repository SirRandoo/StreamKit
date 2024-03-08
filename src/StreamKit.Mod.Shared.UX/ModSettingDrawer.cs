namespace StreamKit.Mod.Shared.UX;

/// <summary>
///     Represents a data class for housing information about a setting for the mod.
/// </summary>
/// <param name="Label">The label for the setting.</param>
/// <param name="Drawer">
///     A <see cref="ITypeDrawer" /> instance that's used to draw the setting's value on screen.
/// </param>
/// <param name="Order">The order of the setting within the menu the setting is being drawn in.</param>
/// <param name="Description">
///     The optional description of the setting used to provide more information about what the setting
///     is to the user.
/// </param>
public record ModSettingDrawer(string Label, ITypeDrawer Drawer, int Order, string? Description = null);
