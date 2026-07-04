using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class MaxLengthValidatorTests
{
    [Fact]
    public void Constructor_throws_for_negative_length()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MaxLengthValidator(-1));
    }

    [Fact]
    public void Exposes_max_length()
    {
        var validator = new MaxLengthValidator(3);

        Assert.Equal(3, validator.MaxLength);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    public void Validate_returns_Valid_for_a_short_enough_value(string value)
    {
        var validator = new MaxLengthValidator(3);

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate(value));
    }

    [Fact]
    public void Validate_returns_Invalid_for_a_too_long_value()
    {
        var validator = new MaxLengthValidator(3);

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("abcd"));
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new MaxLengthValidator(3);

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("abcd"));
        Assert.Equal("Value must be at most 3 characters long.", result.ErrorMessage);
    }
}
