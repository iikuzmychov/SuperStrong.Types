namespace SuperStrong.Types.Validators;

public sealed class UpperInvariantValidator : StrongTypeValidator<string>
{
    public override StrongTypeValidatorResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.ToUpperInvariant() != value)
        {
            return StrongTypeValidatorResult.Invalid("Value must be in upper invariant case.");
        }

        return StrongTypeValidatorResult.Valid();
    }
}
