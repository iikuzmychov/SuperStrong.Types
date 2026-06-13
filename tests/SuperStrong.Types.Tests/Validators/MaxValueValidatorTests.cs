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
    public void Inclusive_allows_the_boundary_value()
    {
        var validator = new MaxValueValidator<int>(5);

        Assert.True(validator.IsValid(5));
    }

    [Fact]
    public void Exclusive_rejects_the_boundary_value()
    {
        var validator = new MaxValueValidator<int>(5, isExclusive: true);

        Assert.False(validator.IsValid(5));
    }

    [Fact]
    public void Exclusive_allows_a_value_below_the_boundary()
    {
        var validator = new MaxValueValidator<int>(5, isExclusive: true);

        Assert.True(validator.IsValid(4));
    }

    [Fact]
    public void Rejects_a_value_above_the_boundary()
    {
        var validator = new MaxValueValidator<int>(5, isExclusive: true);

        Assert.False(validator.IsValid(6));
    }
}
