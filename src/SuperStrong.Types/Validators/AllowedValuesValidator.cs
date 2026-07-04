using System.Collections.Immutable;

namespace SuperStrong.Types.Validators;

public sealed class AllowedValuesValidator<TPrimitive> : StrongTypeValidator<TPrimitive>
    where TPrimitive : notnull
{
    public ImmutableHashSet<TPrimitive> AllowedValues { get; }

    public AllowedValuesValidator(ImmutableHashSet<TPrimitive> allowedValues)
    {
        if (allowedValues.IsEmpty)
        {
            throw new ArgumentException("At least one allowed value must be specified.", nameof(allowedValues));
        }

        AllowedValues = allowedValues;
    }

    public override StrongTypeValidationResult Validate(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!AllowedValues.Contains(value))
        {
            return StrongTypeValidationResult.Invalid($"Value must be one of: {string.Join(", ", AllowedValues)}.");
        }

        return StrongTypeValidationResult.Valid();
    }
}
