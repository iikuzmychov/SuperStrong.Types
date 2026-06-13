using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class NotWhiteSpaceValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("  \r\n ")]
    public void IsValid_returns_false_for_empty_or_white_space(string value)
    {
        var validator = new NotWhiteSpaceValidator();

        Assert.False(validator.IsValid(value));
    }

    [Theory]
    [InlineData("a")]
    [InlineData(" a ")]
    [InlineData("hello world")]
    public void IsValid_returns_true_for_non_white_space(string value)
    {
        var validator = new NotWhiteSpaceValidator();

        Assert.True(validator.IsValid(value));
    }

    [Fact]
    public void EnsureValid_throws_for_white_space()
    {
        var validator = new NotWhiteSpaceValidator();

        Assert.Throws<ArgumentException>(() => validator.EnsureValid("   "));
    }
}
