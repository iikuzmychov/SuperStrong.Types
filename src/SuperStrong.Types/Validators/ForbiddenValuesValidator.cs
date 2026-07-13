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

    public override StrongTypeValidatorResult Validate(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (ForbiddenValues.Contains(value))
        {
            return StrongTypeValidatorResult.Invalid($"Value must not be one of: {string.Join(", ", ForbiddenValues)}.");
        }

        return StrongTypeValidatorResult.Valid();
    }
}
