using System.Text.RegularExpressions;

namespace SuperStrong.Types.Validators;

public sealed class RegexValidator : StrongTypeValidator<string>
{
    public Regex Regex { get; }

    public RegexValidator(Regex regex)
    {
        ArgumentNullException.ThrowIfNull(regex);

        Regex = regex;
    }

    protected override Exception? GetValidationException(string value)
    {
        if (!Regex.IsMatch(value))
        {
            return new ArgumentException($"Value does not match the required pattern '{Regex}'.", nameof(value));
        }

        return null;
    }
}
