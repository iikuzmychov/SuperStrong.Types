using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class ExactLengthValidatorTests
{
    [Fact]
    public void Constructor_throws_for_negative_length()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ExactLengthValidator(-1));
    }

    [Fact]
    public void Exposes_length()
    {
        var validator = new ExactLengthValidator(3);

        Assert.Equal(3, validator.Length);
    }

    [Fact]
    public void Validate_returns_Valid_for_a_value_of_the_exact_length()
    {
        var validator = new ExactLengthValidator(3);

        Assert.True(validator.Validate("abc").IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    [InlineData("abcd")]
    public void Validate_returns_Invalid_for_a_value_of_a_different_length(string value)
    {
        var validator = new ExactLengthValidator(3);

        Assert.False(validator.Validate(value).IsValid);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new ExactLengthValidator(3);

        var result = validator.Validate("abcd");

        Assert.False(result.IsValid);
        Assert.Equal("Value must be exactly 3 characters long.", result.ErrorMessage);
    }
}
