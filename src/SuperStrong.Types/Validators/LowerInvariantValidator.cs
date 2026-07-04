namespace SuperStrong.Types.Validators;

public sealed class LowerInvariantValidator : StrongTypeValidator<string>
{
    public override StrongTypeValidationResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.ToLowerInvariant() != value)
        {
            return StrongTypeValidationResult.Invalid("Value must be in lower invariant case.");
        }

        return StrongTypeValidationResult.Valid();
    }
}
