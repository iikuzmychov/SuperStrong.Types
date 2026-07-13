using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class MinLengthValidatorTests
{
    [Fact]
    public void Constructor_throws_for_negative_length()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MinLengthValidator(-1));
    }

    [Fact]
    public void Exposes_min_length()
    {
        var validator = new MinLengthValidator(3);

        Assert.Equal(3, validator.MinLength);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("abcd")]
    public void Validate_returns_Valid_for_a_long_enough_value(string value)
    {
        var validator = new MinLengthValidator(3);

        Assert.True(validator.Validate(value).IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public void Validate_returns_Invalid_for_a_too_short_value(string value)
    {
        var validator = new MinLengthValidator(3);

        Assert.False(validator.Validate(value).IsValid);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new MinLengthValidator(3);

        var result = validator.Validate("ab");

        Assert.False(result.IsValid);
        Assert.Equal("Value must be at least 3 characters long.", result.ErrorMessage);
    }
}
