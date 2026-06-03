using System.Numerics;

namespace SuperStrong.Types.Validators;

public sealed class MinValueValidator<TPrimitive>(TPrimitive minValue) : StrongTypeValidator<TPrimitive>
    where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
{
    public TPrimitive MinValue { get; } = minValue ?? throw new ArgumentNullException(nameof(minValue));

    protected override Exception? GetValidationException(TPrimitive value)
    {
        if (value < MinValue)
        {
            return new ArgumentOutOfRangeException($"Value must be greater than or equal to {MinValue}.");
        }

        return null;
    }
}
