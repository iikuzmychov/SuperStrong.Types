namespace SuperStrong.Types.Validators;

public sealed class MaxLengthValidator : StrongTypeValidator<string>
{
    public int MaxLength { get; }

    public MaxLengthValidator(int maxLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxLength, 0);
        
        MaxLength = maxLength;
    }

    public override StrongTypeValidatorResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length > MaxLength)
        {
            return StrongTypeValidatorResult.Invalid($"Value must be at most {MaxLength} characters long.");
        }

        return StrongTypeValidatorResult.Valid();
    }
}
