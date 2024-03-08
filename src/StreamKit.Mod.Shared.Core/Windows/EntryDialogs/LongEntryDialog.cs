using System;
using System.Globalization;

namespace StreamKit.Mod.Shared.Core.Windows;

public class LongEntryDialog : NumberEntryDialog<long>
{
    /// <inheritdoc />
    public LongEntryDialog(Action<long> setter, long minimum = 0, long maximum = long.MaxValue) : base(setter, minimum, maximum)
    {
    }

    /// <inheritdoc />
    protected override string FormatNumber(long value) => value.ToString("N0", NumberFormatInfo.CurrentInfo);

    /// <inheritdoc />
    protected override bool TryParseNumber(string value, out long number) => long.TryParse(
        value,
        NumberStyles.Integer | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
        NumberFormatInfo.CurrentInfo,
        out number
    );
}
