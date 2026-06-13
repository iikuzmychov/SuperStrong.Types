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

    protected override Exception? GetValidationException(TPrimitive value)
    {
        if (!AllowedValues.Contains(value))
        {
            return new ArgumentException("Value is not one of the allowed values.", nameof(value));
        }

        return null;
    }
}
