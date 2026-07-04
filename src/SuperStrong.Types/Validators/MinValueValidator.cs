namespace SuperStrong.Types.Validators;

public sealed class MinValueValidator<TPrimitive>(TPrimitive minValue, bool isExclusive = false) : StrongTypeValidator<TPrimitive>
    where TPrimitive : IComparable<TPrimitive>
{
    public TPrimitive MinValue { get; } = minValue ?? throw new ArgumentNullException(nameof(minValue));
    public bool IsExclusive { get; } = isExclusive;

    public override StrongTypeValidationResult Validate(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var comparisonResult = value.CompareTo(MinValue);

        var isBelowBound = IsExclusive
            ? comparisonResult <= 0
            : comparisonResult < 0;

        if (isBelowBound)
        {
            var boundDescription = IsExclusive ? "greater than" : "greater than or equal to";

            return StrongTypeValidationResult.Invalid($"Value must be {boundDescription} {MinValue}.");
        }

        return StrongTypeValidationResult.Valid();
    }
}
