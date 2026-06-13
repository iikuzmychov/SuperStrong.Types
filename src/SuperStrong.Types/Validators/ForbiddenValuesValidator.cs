using System.Collections.Immutable;

namespace SuperStrong.Types.Validators;

public sealed class ForbiddenValuesValidator<TPrimitive> : StrongTypeValidator<TPrimitive>
    where TPrimitive : notnull
{
    public ImmutableHashSet<TPrimitive> ForbiddenValues { get; }

    public ForbiddenValuesValidator(ImmutableHashSet<TPrimitive> forbiddenValues)
    {
        if (forbiddenValues.IsEmpty)
        {
            throw new ArgumentException("At least one forbidden value must be specified.", nameof(forbiddenValues));
        }

        ForbiddenValues = forbiddenValues;
    }

    protected override Exception? GetValidationException(TPrimitive value)
    {
        if (ForbiddenValues.Contains(value))
        {
            return new ArgumentException("Value is one of the forbidden values.", nameof(value));
        }

        return null;
    }
}
