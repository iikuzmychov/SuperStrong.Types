namespace SuperStrong.Types.Validators;

public sealed class ExactLengthValidator : StrongTypeValidator<string>
{
    public int Length { get; }

    public ExactLengthValidator(int length)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(length, 0);

        Length = length;
    }

    public override StrongTypeValidationResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length != Length)
        {
            return StrongTypeValidationResult.Invalid($"Value must be exactly {Length} characters long.");
        }

        return StrongTypeValidationResult.Valid();
    }
}
