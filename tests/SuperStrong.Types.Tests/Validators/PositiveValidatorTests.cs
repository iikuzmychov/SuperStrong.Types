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

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate(1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_returns_Invalid_for_zero_and_negative_values(int value)
    {
        var validator = new PositiveValidator<int>();

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(value));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Validate_returns_Valid_for_zero_and_positive_values_when_zero_is_allowed(int value)
    {
        var validator = new PositiveValidator<int>(allowZero: true);

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate(value));
    }

    [Fact]
    public void Validate_returns_Invalid_for_a_negative_value_when_zero_is_allowed()
    {
        var validator = new PositiveValidator<int>(allowZero: true);

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(-1));
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new PositiveValidator<int>();

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(0));
        Assert.Equal("Value must be positive.", result.ErrorMessage);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_when_zero_is_allowed()
    {
        var validator = new PositiveValidator<int>(allowZero: true);

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(-1));
        Assert.Equal("Value must be positive or zero.", result.ErrorMessage);
    }
}
