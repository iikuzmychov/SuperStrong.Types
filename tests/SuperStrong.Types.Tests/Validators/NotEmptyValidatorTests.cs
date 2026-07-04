using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class NotEmptyValidatorTests
{
    [Fact]
    public void AllowWhiteSpaces_defaults_to_false()
    {
        var validator = new NotEmptyValidator();

        Assert.False(validator.AllowWhiteSpaces);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("  \r\n ")]
    public void Validate_returns_Invalid_for_an_empty_or_white_space_value(string value)
    {
        var validator = new NotEmptyValidator();

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(value));
    }

    [Theory]
    [InlineData("a")]
    [InlineData(" a ")]
    [InlineData("hello world")]
    public void Validate_returns_Valid_for_a_non_white_space_value(string value)
    {
        var validator = new NotEmptyValidator();

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate(value));
    }

    [Fact]
    public void Validate_returns_Invalid_for_an_empty_value_when_white_spaces_are_allowed()
    {
        var validator = new NotEmptyValidator(allowWhiteSpaces: true);

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(""));
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("a")]
    public void Validate_returns_Valid_for_white_space_values_when_white_spaces_are_allowed(string value)
    {
        var validator = new NotEmptyValidator(allowWhiteSpaces: true);

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate(value));
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new NotEmptyValidator();

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(" "));
        Assert.Equal("Value must not be empty or whitespace.", result.ErrorMessage);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_when_white_spaces_are_allowed()
    {
        var validator = new NotEmptyValidator(allowWhiteSpaces: true);

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(""));
        Assert.Equal("Value must not be empty.", result.ErrorMessage);
    }
}
