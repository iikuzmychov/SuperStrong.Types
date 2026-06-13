namespace SuperStrong.Types.Validators;

public sealed class UpperInvariantValidator : StrongTypeValidator<string>
{
    protected override Exception? GetValidationException(string value)
    {
        if (value != value.ToUpperInvariant())
        {
            return new ArgumentException("Value must be upper-case.", nameof(value));
        }

        return null;
    }
}
