// MIT License
//
// Copyright (c) 2024 SirRandoo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Text.Json.Serialization;
using StreamKit.Mod.Api;

namespace StreamKit.Mod.Core.Settings;

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

    /// <summary>
    ///     The url to the customized version of the mod's "store page," which allows viewers to see what's
    ///     available for sale through the mod.
    /// </summary>
    public Uri? StoreLink { get; set; }

    /// <summary>
    ///     The minimum amount of points a viewer has to spend before the store will "ship" their order.
    /// </summary>
    public int MinimumPurchasePrice { get; set; } = 50;

    /// <summary>
    ///     Whether the store will send "receipts" after every purchase.
    /// </summary>
    public bool PurchaseConfirmations { get; set; } = true;

    /// <summary>
    ///     Whether buildable objects are available for purchase within the store.
    /// </summary>
    /// <remarks>
    ///     Buildable objects typically represent buildings, such as walls, generators, and monuments.
    /// </remarks>
    public bool BuildingsPurchasable { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("_VERSION")]
    public int Version { get; set; } = LatestVersion;
}
