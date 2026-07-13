using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class NegativeValidatorTests
{
    [Fact]
    public void AllowZero_defaults_to_false()
    {
        var validator = new NegativeValidator<int>();

        Assert.False(validator.AllowZero);
    }

    [Fact]
    public void Validate_returns_Valid_for_a_negative_value()
    {
        var validator = new NegativeValidator<int>();

        Assert.True(validator.Validate(-1).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Validate_returns_Invalid_for_zero_and_positive_values(int value)
    {
        var validator = new NegativeValidator<int>();

        Assert.False(validator.Validate(value).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_returns_Valid_for_zero_and_negative_values_when_zero_is_allowed(int value)
    {
        var validator = new NegativeValidator<int>(allowZero: true);

        Assert.True(validator.Validate(value).IsValid);
    }

    [Fact]
    public void Validate_returns_Invalid_for_a_positive_value_when_zero_is_allowed()
    {
        var validator = new NegativeValidator<int>(allowZero: true);

        Assert.False(validator.Validate(1).IsValid);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new NegativeValidator<int>();

        var result = validator.Validate(0);

        Assert.False(result.IsValid);
        Assert.Equal("Value must be negative.", result.ErrorMessage);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_when_zero_is_allowed()
    {
        var validator = new NegativeValidator<int>(allowZero: true);

        var result = validator.Validate(1);

        Assert.False(result.IsValid);
        Assert.Equal("Value must be negative or zero.", result.ErrorMessage);
    }
}
