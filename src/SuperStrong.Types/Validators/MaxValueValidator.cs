namespace SuperStrong.Types.Validators;

public sealed class MaxValueValidator<TPrimitive>(TPrimitive maxValue, bool isExclusive = false) : StrongTypeValidator<TPrimitive>
    where TPrimitive : IComparable<TPrimitive>
{
    public TPrimitive MaxValue { get; } = maxValue ?? throw new ArgumentNullException(nameof(maxValue));
    public bool IsExclusive { get; } = isExclusive;

    public override StrongTypeValidationResult Validate(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var comparisonResult = value.CompareTo(MaxValue);

        var isAboveBound = IsExclusive
            ? comparisonResult >= 0
            : comparisonResult > 0;

        if (isAboveBound)
        {
            var boundDescription = IsExclusive ? "less than" : "less than or equal to";

            return StrongTypeValidationResult.Invalid($"Value must be {boundDescription} {MaxValue}.");
        }

        return StrongTypeValidationResult.Valid();
    }
}
