namespace SuperStrong.Types.Validators;

public sealed class NotEmptyValidator(bool allowWhiteSpaces = false) : StrongTypeValidator<string>
{
    public bool AllowWhiteSpaces { get; } = allowWhiteSpaces;

    public override StrongTypeValidatorResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var isInvalid = AllowWhiteSpaces
            ? string.IsNullOrEmpty(value)
            : string.IsNullOrWhiteSpace(value);

        if (isInvalid)
        {
            var message = AllowWhiteSpaces
                ? "Value must not be empty."
                : "Value must not be empty or whitespace.";

            return StrongTypeValidatorResult.Invalid(message);
        }

        return StrongTypeValidatorResult.Valid();
    }
}
