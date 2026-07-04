using SuperStrong.Types.Validators;
using System.Text.RegularExpressions;

namespace SuperStrong.Types.Tests.Validators;

public sealed class RegexValidatorTests
{
    [Fact]
    public void Constructor_throws_for_null_regex()
    {
        Assert.Throws<ArgumentNullException>(() => new RegexValidator(null!));
    }

    [Fact]
    public void Exposes_regex()
    {
        var regex = new Regex("^[a-z]+$");

        var validator = new RegexValidator(regex);

        Assert.Same(regex, validator.Regex);
    }

    [Fact]
    public void Validate_returns_Valid_when_pattern_matches()
    {
        var validator = new RegexValidator(new Regex("^[a-z]+$"));

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate("abc"));
    }

    [Fact]
    public void Validate_returns_Invalid_when_pattern_does_not_match()
    {
        var validator = new RegexValidator(new Regex("^[a-z]+$"));

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("ABC"));
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_with_the_pattern()
    {
        var validator = new RegexValidator(new Regex("^[a-z]+$"));

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("123"));
        Assert.Equal("Value must match the regex pattern '^[a-z]+$'.", result.ErrorMessage);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_with_the_regex_options()
    {
        var validator = new RegexValidator(new Regex("^[a-z]+$", RegexOptions.Multiline));

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("123"));
        Assert.Equal("Value must match the regex pattern '^[a-z]+$' (Multiline).", result.ErrorMessage);
    }
}
