namespace SuperStrong.Types.Validators;

public sealed class LowerInvariantValidator : StrongTypeValidator<string>
{
    public override StrongTypeValidatorResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.ToLowerInvariant() != value)
        {
            return StrongTypeValidatorResult.Invalid("Value must be in lower invariant case.");
        }

        return StrongTypeValidatorResult.Valid();
    }
}
