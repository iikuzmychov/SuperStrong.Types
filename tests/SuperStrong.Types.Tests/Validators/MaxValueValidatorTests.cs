using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class MaxValueValidatorTests
{
    [Fact]
    public void IsExclusive_defaults_to_false()
    {
        var validator = new MaxValueValidator<int>(5);

        Assert.False(validator.IsExclusive);
    }

    [Fact]
    public void Validate_returns_Valid_for_the_boundary_value()
    {
        var validator = new MaxValueValidator<int>(5);

        Assert.True(validator.Validate(5).IsValid);
    }

    [Fact]
    public void Validate_returns_Invalid_for_the_boundary_value_when_exclusive()
    {
        var validator = new MaxValueValidator<int>(5, isExclusive: true);

        Assert.False(validator.Validate(5).IsValid);
    }

    [Fact]
    public void Validate_returns_Valid_for_a_value_below_the_boundary_when_exclusive()
    {
        var validator = new MaxValueValidator<int>(5, isExclusive: true);

        Assert.True(validator.Validate(4).IsValid);
    }

    [Fact]
    public void Validate_returns_Invalid_for_a_value_above_the_boundary()
    {
        var validator = new MaxValueValidator<int>(5);

        Assert.False(validator.Validate(6).IsValid);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new MaxValueValidator<int>(5);

        var result = validator.Validate(6);

        Assert.False(result.IsValid);
        Assert.Equal("Value must be less than or equal to 5.", result.ErrorMessage);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_when_exclusive()
    {
        var validator = new MaxValueValidator<int>(5, isExclusive: true);

        var result = validator.Validate(5);

        Assert.False(result.IsValid);
        Assert.Equal("Value must be less than 5.", result.ErrorMessage);
    }
}
