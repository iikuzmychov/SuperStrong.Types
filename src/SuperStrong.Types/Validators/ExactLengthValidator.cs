namespace SuperStrong.Types.Validators;

public sealed class ExactLengthValidator : StrongTypeValidator<string>
{
    public int Length { get; }

    public ExactLengthValidator(int length)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(length, 0);

        Length = length;
    }

    public override StrongTypeValidatorResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length != Length)
        {
            return StrongTypeValidatorResult.Invalid($"Value must be exactly {Length} characters long.");
        }

        return StrongTypeValidatorResult.Valid();
    }
}
