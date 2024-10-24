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

using System.Collections.Generic;
using StreamKit.Shared.Interfaces;
using StreamKit.UX.Drawers;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.UX;

public class TransactionTableDrawer : TableDrawer<ITransaction>
{
    private TransactionTableDrawer()
    {
    }

    /// <inheritdoc />
    protected override void DrawRowEntry(Rect region, ITransaction data, IReadOnlyList<Rect> columnRegions)
    {
        Rect nameColumnRegion = columnRegions[0];
        Rect typeColumnRegion = columnRegions[1];
        Rect amountColumnRegion = columnRegions[2];

        var rowRegion = new Rect(region.x, region.y, amountColumnRegion.x + amountColumnRegion.width, nameColumnRegion.height);
        var nameRegion = new Rect(nameColumnRegion.x, rowRegion.y, nameColumnRegion.width, rowRegion.height);
        var typeRegion = new Rect(typeColumnRegion.x, rowRegion.y, typeColumnRegion.width, rowRegion.height);
        var amountRegion = new Rect(amountColumnRegion.x, rowRegion.y, amountColumnRegion.width, rowRegion.height);

        LabelDrawer.DrawLabel(nameRegion, data.Name);
        LabelDrawer.DrawLabel(typeRegion, data.ProductId.ToUpper());

        var priceString = data.Amount.ToString("N0");

        if (data.Refunded)
        {
            priceString = $"+{priceString}";
        }

        LabelDrawer.DrawLabel(amountRegion, priceString, TextAnchor.MiddleRight);

        // FIXME: This formats a new string every draw frame for static information.
        TooltipHandler.TipRegion(rowRegion, string.Format(KitTranslations.TransactionIdTableTooltip, data.Id));

        // FIXME: This formats a new string every draw frame for static information.
        TooltipHandler.TipRegion(rowRegion, string.Format(KitTranslations.TransactionPurchasedOnTableTooltip, data.OccurredAt.ToString("F")));

        if (data.Refunded)
        {
            TooltipHandler.TipRegion(rowRegion, KitTranslations.TransactionRefundedOnTableTooltip);
        }
    }

    public static Builder<TransactionTableDrawer> BuildInstance() => new(() => new TransactionTableDrawer());
}
