using SuperStrong.Types.Validators;
using System.Text.RegularExpressions;

namespace SuperStrong.Types.Tests.Validators;

public sealed class RegexValidatorTests
{
    [Fact]
    public void Constructor_throws_for_null_regex()
    {
        Assert.Throws<ArgumentNullException>(() => new RegexValidator(null!));
    }

    [Fact]
    public void Exposes_the_regex()
    {
        var regex = new Regex("^[a-z]+$");

        var validator = new RegexValidator(regex);

        Assert.Same(regex, validator.Regex);
    }

    [Fact]
    public void IsValid_returns_true_when_pattern_matches()
    {
        var validator = new RegexValidator(new Regex("^[a-z]+$"));

        Assert.True(validator.IsValid("abc"));
    }

    [Fact]
    public void IsValid_returns_false_when_pattern_does_not_match()
    {
        var validator = new RegexValidator(new Regex("^[a-z]+$"));

        Assert.False(validator.IsValid("ABC"));
    }

    [Fact]
    public void EnsureValid_throws_when_pattern_does_not_match()
    {
        var validator = new RegexValidator(new Regex("^[a-z]+$"));

        Assert.Throws<ArgumentException>(() => validator.EnsureValid("123"));
    }
}
