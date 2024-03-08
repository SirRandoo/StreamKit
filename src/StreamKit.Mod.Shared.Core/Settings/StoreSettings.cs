using System;
using StreamKit.Mod.Api;
using StreamKit.Mod.Api.Attributes;

namespace StreamKit.Mod.Shared.Core.Settings;

/// <summary>
///     <para>
///         A class for housing store settings.
///     </para>
///     <para>
///         Store settings are settings that affect the inventory of the mod's store and any
///         transactions viewers make through the store.
///     </para>
/// </summary>
public class StoreSettings : IComponentSettings
{
    /// <summary>
    ///     The latest version of the store settings.
    /// </summary>
    /// <remarks>
    ///     This constant is used in-tandem with <see cref="Version" /> to convert older settings into a
    ///     newer format.
    /// </remarks>
    public const int LatestVersion = 1;

    [Label("Store website")]
    [Description("This setting is used by the mod to display a link to a list of products available for purchase.")]
    public Uri? StoreLink { get; set; }

    /// <summary>
    ///     The minimum amount of points a viewer has to spend before the store will "ship" their order.
    /// </summary>
    [Label("Minimum purchase price")]
    [Description("The minimum amount of points viewers have to send before the store will accept an order.")]
    public int MinimumPurchasePrice { get; set; } = 50;

    /// <summary>
    ///     Whether the store will send "receipts" after every purchase.
    /// </summary>
    [Label("Send purchase confirmations")]
    [Description("When enabled, all purchases will send a confirmation message in chat stating the product purchased and the price paid.")]
    public bool PurchaseConfirmations { get; set; } = true;

    /// <summary>
    ///     Whether buildable objects are available for purchase within the store.
    /// </summary>
    /// <remarks>
    ///     Buildable objects typically represent buildings, such as walls, generators, and monuments.
    /// </remarks>
    [Label("Allow viewers to purchase buildings")]
    [Description("When enabled, viewers will be able to purchase buildings, as well as any other 'minifiable' things.")]
    public bool BuildingsPurchasable { get; set; }

    /// <inheritdoc />
    [InternalSetting]
    public int Version { get; set; } = LatestVersion;
}
