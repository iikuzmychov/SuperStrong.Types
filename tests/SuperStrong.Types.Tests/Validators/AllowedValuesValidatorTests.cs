using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.Tests.Validators;

public sealed class AllowedValuesValidatorTests
{
    [Fact]
    public void Constructor_throws_for_an_empty_set()
    {
        Assert.Throws<ArgumentException>(() => new AllowedValuesValidator<int>([]));
    }

    [Fact]
    public void Exposes_allowed_values()
    {
        var validator = new AllowedValuesValidator<int>([1, 2, 3]);

        Assert.True(validator.AllowedValues.SetEquals([1, 2, 3]));
    }

    [Fact]
    public void Validate_returns_Valid_for_an_allowed_value()
    {
        var validator = new AllowedValuesValidator<string>(["red", "green", "blue"]);

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate("green"));
    }

    [Fact]
    public void Validate_returns_Invalid_for_a_value_outside_the_set()
    {
        var validator = new AllowedValuesValidator<string>(["red", "green", "blue"]);

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("yellow"));
    }

    [Fact]
    public void Validate_is_case_sensitive_for_strings()
    {
        var validator = new AllowedValuesValidator<string>(["red"]);

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("RED"));
    }

    [Fact]
    public void Validate_honors_the_comparer_carried_by_the_set()
    {
        var validator = new AllowedValuesValidator<string>(ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase, "red"));

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate("RED"));
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_with_the_allowed_values()
    {
        var validator = new AllowedValuesValidator<string>(["red"]);

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("yellow"));
        Assert.Equal("Value must be one of: red.", result.ErrorMessage);
    }
}
