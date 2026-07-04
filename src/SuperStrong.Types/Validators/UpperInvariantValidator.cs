namespace SuperStrong.Types.Validators;

public sealed class UpperInvariantValidator : StrongTypeValidator<string>
{
    public override StrongTypeValidationResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.ToUpperInvariant() != value)
        {
            return StrongTypeValidationResult.Invalid("Value must be in upper invariant case.");
        }

        return StrongTypeValidationResult.Valid();
    }
}
