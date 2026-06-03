using System.Numerics;

namespace SuperStrong.Types.Validators;

public sealed class MaxValueValidator<TPrimitive>(TPrimitive maxValue) : StrongTypeValidator<TPrimitive>
    where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
{
    public TPrimitive MaxValue { get; } = maxValue ?? throw new ArgumentNullException(nameof(maxValue));

    protected override Exception? GetValidationException(TPrimitive value)
    {
        if (value > MaxValue)
        {
            return new ArgumentOutOfRangeException(nameof(value));
        }

        return null;
    }
}
