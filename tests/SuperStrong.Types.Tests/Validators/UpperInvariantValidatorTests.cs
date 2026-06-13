using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class UpperInvariantValidatorTests
{
    [Theory]
    [InlineData("ABC")]
    [InlineData("ABC123")]
    [InlineData("A-B_C")]
    [InlineData("")]
    public void IsValid_returns_true_when_there_are_no_lower_case_characters(string value)
    {
        var validator = new UpperInvariantValidator();

        Assert.True(validator.IsValid(value));
    }

    [Theory]
    [InlineData("Abc")]
    [InlineData("abc")]
    [InlineData("AbC")]
    public void IsValid_returns_false_when_a_lower_case_character_is_present(string value)
    {
        var validator = new UpperInvariantValidator();

        Assert.False(validator.IsValid(value));
    }

    [Fact]
    public void EnsureValid_throws_for_a_lower_case_character()
    {
        var validator = new UpperInvariantValidator();

        Assert.Throws<ArgumentException>(() => validator.EnsureValid("aBC"));
    }
}
