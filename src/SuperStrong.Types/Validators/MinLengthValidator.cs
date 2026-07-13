namespace SuperStrong.Types.Validators;

public sealed class MinLengthValidator : StrongTypeValidator<string>
{
    public int MinLength { get; }

    public MinLengthValidator(int minLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(minLength, 0);

        MinLength = minLength;
    }

    public override StrongTypeValidatorResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length < MinLength)
        {
            return StrongTypeValidatorResult.Invalid($"Value must be at least {MinLength} characters long.");
        }

        return StrongTypeValidatorResult.Valid();
    }
}
