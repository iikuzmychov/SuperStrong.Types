namespace SuperStrong.Types.Validators;

public sealed class NotWhiteSpaceValidator : StrongTypeValidator<string>
{
    protected override Exception? GetValidationException(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ArgumentException(
                "Value must not be empty or consist only of white-space characters.",
                nameof(value));
        }

        return null;
    }
}
