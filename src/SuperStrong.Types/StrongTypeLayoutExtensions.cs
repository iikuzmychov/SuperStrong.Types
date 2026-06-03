using System.Numerics;

namespace SuperStrong.Types;

public static class StrongTypeLayoutExtensions
{
    public static StrongTypeLayout<TPrimitive> HasComparisonOperators<TPrimitive>(
        this StrongTypeLayout<TPrimitive> layout,
        bool generate)
        where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
    {
        ArgumentNullException.ThrowIfNull(layout);

        // layout is compile-time feature used by source generator, no runtime behavior
        return layout;
    }
}
