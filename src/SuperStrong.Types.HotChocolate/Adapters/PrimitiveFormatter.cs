using System.Globalization;

namespace SuperStrong.Types.HotChocolate.Adapters;

internal static class PrimitiveFormatter
{
    public static string Format<TPrimitive>(TPrimitive value)
        where TPrimitive : notnull
    {
        return value is IFormattable formattable
            ? formattable.ToString(null, CultureInfo.InvariantCulture)
            : value.ToString()!;
    }
}
