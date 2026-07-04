namespace SuperStrong.Types.Validators;

public sealed class MinLengthValidator : StrongTypeValidator<string>
{
    public int MinLength { get; }

    public MinLengthValidator(int minLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(minLength, 0);

        MinLength = minLength;
    }

    public override StrongTypeValidationResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length < MinLength)
        {
            return StrongTypeValidationResult.Invalid($"Value must be at least {MinLength} characters long.");
        }

        return StrongTypeValidationResult.Valid();
    }
}
