using System.Numerics;

namespace SuperStrong.Types.Validators;

public sealed class MaxValueValidator<TPrimitive>(TPrimitive maxValue, bool isExclusive = false) : StrongTypeValidator<TPrimitive>
    where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
{
    public TPrimitive MaxValue { get; } = maxValue ?? throw new ArgumentNullException(nameof(maxValue));
    public bool IsExclusive { get; } = isExclusive;

    protected override Exception? GetValidationException(TPrimitive value)
    {
        var isAboveBound = IsExclusive
            ? value >= MaxValue
            : value > MaxValue;

        if (isAboveBound)
        {
            var boundDescription = IsExclusive ? "less than" : "less than or equal to";

            return new ArgumentOutOfRangeException($"Value must be {boundDescription} {MaxValue}.");
        }

        return null;
    }
}
