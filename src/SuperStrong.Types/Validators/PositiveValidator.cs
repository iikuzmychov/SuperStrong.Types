using System.Numerics;

namespace SuperStrong.Types.Validators;

public sealed class PositiveValidator<TPrimitive>(bool allowZero = false) : StrongTypeValidator<TPrimitive>
    where TPrimitive : INumberBase<TPrimitive>, IComparable<TPrimitive>
{
    public bool AllowZero { get; } = allowZero;

    public override StrongTypeValidatorResult Validate(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var comparisonResult = value.CompareTo(TPrimitive.Zero);

        var isInvalid = AllowZero
            ? comparisonResult < 0
            : comparisonResult <= 0;

        if (isInvalid)
        {
            var message = AllowZero
                ? "Value must be positive or zero."
                : "Value must be positive.";

            return StrongTypeValidatorResult.Invalid(message);
        }

        return StrongTypeValidatorResult.Valid();
    }
}
