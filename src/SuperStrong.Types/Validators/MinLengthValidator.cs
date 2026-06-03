namespace SuperStrong.Types.Validators;

public sealed class MinLengthValidator : StrongTypeValidator<string>
{
    public int MinLength { get; }

    public MinLengthValidator(int minLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(minLength, 0, nameof(minLength));

        MinLength = minLength;
    }

    protected override Exception? GetValidationException(string value)
    {
        if (value.Length < MinLength)
        {
            return new ArgumentOutOfRangeException(
                nameof(value),
                $"Value length must be greater than or equal to {MinLength}.");
        }

        return null;
    }
}
