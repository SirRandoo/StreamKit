using System;
using System.Globalization;

namespace StreamKit.Mod.Shared.Core.Windows;

public class IntEntryDialog : NumberEntryDialog<int>
{
    /// <inheritdoc />
    public IntEntryDialog(Action<int> setter, int minimum = 0, int maximum = int.MaxValue) : base(setter, minimum, maximum)
    {
    }

    /// <inheritdoc />
    protected override string FormatNumber(int value) => value.ToString("N0", NumberFormatInfo.CurrentInfo);

    /// <inheritdoc />
    protected override bool TryParseNumber(string value, out int number) => int.TryParse(
        value,
        NumberStyles.Integer | NumberStyles.AllowThousands | NumberStyles.Currency | NumberStyles.AllowExponent,
        NumberFormatInfo.CurrentInfo,
        out number
    );
}
