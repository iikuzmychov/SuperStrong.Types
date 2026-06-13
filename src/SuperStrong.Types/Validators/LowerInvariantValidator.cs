namespace SuperStrong.Types.Validators;

public sealed class LowerInvariantValidator : StrongTypeValidator<string>
{
    protected override Exception? GetValidationException(string value)
    {
        if (value != value.ToLowerInvariant())
        {
            return new ArgumentException("Value must be lower-case.", nameof(value));
        }

        return null;
    }
}
