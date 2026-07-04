using SuperStrong.Types.Validators;

namespace SuperStrong.Types.Tests.Validators;

public sealed class UpperInvariantValidatorTests
{
    [Theory]
    [InlineData("ABC")]
    [InlineData("ABC123")]
    [InlineData("A-B_C")]
    [InlineData("")]
    public void Validate_returns_Valid_when_there_are_no_lower_case_characters(string value)
    {
        var validator = new UpperInvariantValidator();

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate(value));
    }

    [Theory]
    [InlineData("Abc")]
    [InlineData("abc")]
    [InlineData("AbC")]
    public void Validate_returns_Invalid_when_a_lower_case_character_is_present(string value)
    {
        var validator = new UpperInvariantValidator();

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate(value));
    }

    [Fact]
    public void Invalid_result_carries_an_error_message()
    {
        var validator = new UpperInvariantValidator();

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("aBC"));
        Assert.Equal("Value must be in upper invariant case.", result.ErrorMessage);
    }
}
