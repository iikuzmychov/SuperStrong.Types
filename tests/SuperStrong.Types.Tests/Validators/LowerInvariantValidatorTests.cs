using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class LowerInvariantValidatorTests
{
    [Theory]
    [InlineData("abc")]
    [InlineData("abc123")]
    [InlineData("a-b_c")]
    [InlineData("")]
    public void IsValid_returns_true_when_there_are_no_upper_case_characters(string value)
    {
        var validator = new LowerInvariantValidator();

        Assert.True(validator.IsValid(value));
    }

    [Theory]
    [InlineData("Abc")]
    [InlineData("ABC")]
    [InlineData("aBc")]
    public void IsValid_returns_false_when_an_upper_case_character_is_present(string value)
    {
        var validator = new LowerInvariantValidator();

        Assert.False(validator.IsValid(value));
    }

    [Fact]
    public void EnsureValid_throws_for_an_upper_case_character()
    {
        var validator = new LowerInvariantValidator();

        Assert.Throws<ArgumentException>(() => validator.EnsureValid("Abc"));
    }
}
