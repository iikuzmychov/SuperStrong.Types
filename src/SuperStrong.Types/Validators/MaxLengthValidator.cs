namespace SuperStrong.Types.Validators;

public sealed class MaxLengthValidator : StrongTypeValidator<string>
{
    public int MaxLength { get; }

    public MaxLengthValidator(int maxLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxLength, 0, nameof(maxLength));
        
        MaxLength = maxLength;
    }

    protected override Exception? GetValidationException(string value)
    {
        if (value.Length > MaxLength)
        {
            return new ArgumentOutOfRangeException(
                nameof(value),
                $"Value length must be less than or equal to {MaxLength}.");
        }

        return null;
    }
}
