using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class LowerInvariantValidatorTests
{
    [Theory]
    [InlineData("abc")]
    [InlineData("abc123")]
    [InlineData("a-b_c")]
    [InlineData("")]
    public void Validate_returns_Valid_when_there_are_no_upper_case_characters(string value)
    {
        var validator = new LowerInvariantValidator();

        Assert.True(validator.Validate(value).IsValid);
    }

    [Theory]
    [InlineData("Abc")]
    [InlineData("ABC")]
    [InlineData("aBc")]
    public void Validate_returns_Invalid_when_an_upper_case_character_is_present(string value)
    {
        var validator = new LowerInvariantValidator();

        Assert.False(validator.Validate(value).IsValid);
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new LowerInvariantValidator();

        var result = validator.Validate("Abc");

        Assert.False(result.IsValid);
        Assert.Equal("Value must be in lower invariant case.", result.ErrorMessage);
    }
}
