namespace SuperStrong.Types.Validators;

public sealed class MaxLengthValidator : StrongTypeValidator<string>
{
    public int MaxLength { get; }

    public MaxLengthValidator(int maxLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxLength, 0);
        
        MaxLength = maxLength;
    }

    public override StrongTypeValidationResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length > MaxLength)
        {
            return StrongTypeValidationResult.Invalid($"Value must be at most {MaxLength} characters long.");
        }

        return StrongTypeValidationResult.Valid();
    }
}
