using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class MinValueValidatorTests
{
    [Fact]
    public void IsExclusive_defaults_to_false()
    {
        var validator = new MinValueValidator<int>(5);

        Assert.False(validator.IsExclusive);
    }

    [Fact]
    public void Validate_returns_Valid_for_the_boundary_value()
    {
        var validator = new MinValueValidator<int>(5);

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate(5));
    }

    [Fact]
    public void Validate_returns_Invalid_for_the_boundary_value_when_exclusive()
    {
        var validator = new MinValueValidator<int>(5, isExclusive: true);

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(5));
    }

    [Fact]
    public void Validate_returns_Valid_for_a_value_above_the_boundary_when_exclusive()
    {
        var validator = new MinValueValidator<int>(5, isExclusive: true);

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate(6));
    }

    [Fact]
    public void Validate_returns_Invalid_for_a_value_below_the_boundary()
    {
        var validator = new MinValueValidator<int>(5);

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(4));
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new MinValueValidator<int>(5);

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(4));
        Assert.Equal("Value must be greater than or equal to 5.", result.ErrorMessage);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_when_exclusive()
    {
        var validator = new MinValueValidator<int>(5, isExclusive: true);

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(5));
        Assert.Equal("Value must be greater than 5.", result.ErrorMessage);
    }
}
