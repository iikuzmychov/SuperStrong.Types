using System.Numerics;

namespace SuperStrong.Types.Validators;

public sealed class MinValueValidator<TPrimitive>(TPrimitive minValue, bool isExclusive = false) : StrongTypeValidator<TPrimitive>
    where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
{
    public TPrimitive MinValue { get; } = minValue ?? throw new ArgumentNullException(nameof(minValue));
    public bool IsExclusive { get; } = isExclusive;

    protected override Exception? GetValidationException(TPrimitive value)
    {
        var isBelowBound = IsExclusive
            ? value <= MinValue
            : value < MinValue;

        if (isBelowBound)
        {
            var boundDescription = IsExclusive ? "greater than" : "greater than or equal to";

            return new ArgumentOutOfRangeException($"Value must be {boundDescription} {MinValue}.");
        }

        return null;
    }
}
