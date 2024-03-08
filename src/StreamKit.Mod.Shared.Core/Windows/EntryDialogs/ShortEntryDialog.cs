using System;
using System.Globalization;

namespace StreamKit.Mod.Shared.Core.Windows;

public class ShortEntryDialog : NumberEntryDialog<short>
{
    /// <inheritdoc />
    public ShortEntryDialog(Action<short> setter, short minimum = 0, short maximum = short.MaxValue) : base(setter, minimum, maximum)
    {
    }

    /// <inheritdoc />
    protected override string FormatNumber(short value) => value.ToString("N0", NumberFormatInfo.CurrentInfo);

    /// <inheritdoc />
    protected override bool TryParseNumber(string value, out short number) => short.TryParse(
        value,
        NumberStyles.Integer | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
        NumberFormatInfo.CurrentInfo,
        out number
    );
}
