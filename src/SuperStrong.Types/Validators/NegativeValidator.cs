using System.Numerics;

namespace SuperStrong.Types.Validators;

public sealed class NegativeValidator<TPrimitive>(bool allowZero = false) : StrongTypeValidator<TPrimitive>
    where TPrimitive : INumberBase<TPrimitive>, IComparable<TPrimitive>
{
    public bool AllowZero { get; } = allowZero;

    public override StrongTypeValidationResult Validate(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var comparisonResult = value.CompareTo(TPrimitive.Zero);

        var isInvalid = AllowZero
            ? comparisonResult > 0
            : comparisonResult >= 0;

        if (isInvalid)
        {
            var message = AllowZero
                ? "Value must be negative or zero."
                : "Value must be negative.";

            return StrongTypeValidationResult.Invalid(message);
        }

        return StrongTypeValidationResult.Valid();
    }
}
