using System.Text.RegularExpressions;

namespace SuperStrong.Types.Validators;

public sealed class RegexValidator(Regex regex) : StrongTypeValidator<string>
{
    public Regex Regex { get; } = regex ?? throw new ArgumentNullException(nameof(regex));

    public override StrongTypeValidatorResult Validate(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!Regex.IsMatch(value))
        {
            var optionsSuffix = Regex.Options is RegexOptions.None
                ? string.Empty
                : $" ({Regex.Options})";

            return StrongTypeValidatorResult.Invalid($"Value must match the regex pattern '{Regex}'{optionsSuffix}.");
        }

        return StrongTypeValidatorResult.Valid();
    }
}
