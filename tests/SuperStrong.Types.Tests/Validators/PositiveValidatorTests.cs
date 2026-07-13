using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class PositiveValidatorTests
{
    [Fact]
    public void AllowZero_defaults_to_false()
    {
        var validator = new PositiveValidator<int>();

        Assert.False(validator.AllowZero);
    }

    [Fact]
    public void Validate_returns_Valid_for_a_positive_value()
    {
        var validator = new PositiveValidator<int>();

        Assert.True(validator.Validate(1).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_returns_Invalid_for_zero_and_negative_values(int value)
    {
        var validator = new PositiveValidator<int>();

        Assert.False(validator.Validate(value).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Validate_returns_Valid_for_zero_and_positive_values_when_zero_is_allowed(int value)
    {
        var validator = new PositiveValidator<int>(allowZero: true);

        Assert.True(validator.Validate(value).IsValid);
    }

    [Fact]
    public void Validate_returns_Invalid_for_a_negative_value_when_zero_is_allowed()
    {
        var validator = new PositiveValidator<int>(allowZero: true);

        Assert.False(validator.Validate(-1).IsValid);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new PositiveValidator<int>();

        var result = validator.Validate(0);

        Assert.False(result.IsValid);
        Assert.Equal("Value must be positive.", result.ErrorMessage);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_when_zero_is_allowed()
    {
        var validator = new PositiveValidator<int>(allowZero: true);

        var result = validator.Validate(-1);

        Assert.False(result.IsValid);
        Assert.Equal("Value must be positive or zero.", result.ErrorMessage);
    }
}
