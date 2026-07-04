using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.Tests.Validators;

public sealed class ForbiddenValuesValidatorTests
{
    [Fact]
    public void Constructor_throws_for_an_empty_set()
    {
        Assert.Throws<ArgumentException>(() => new ForbiddenValuesValidator<int>([]));
    }

    [Fact]
    public void Exposes_forbidden_values()
    {
        var validator = new ForbiddenValuesValidator<int>([1, 2, 3]);

        Assert.True(validator.ForbiddenValues.SetEquals([1, 2, 3]));
    }

    [Fact]
    public void Validate_returns_Invalid_for_a_forbidden_value()
    {
        var validator = new ForbiddenValuesValidator<string>(["admin", "root"]);

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("admin"));
    }

    [Fact]
    public void Validate_returns_Valid_for_a_value_outside_the_set()
    {
        var validator = new ForbiddenValuesValidator<string>(["admin", "root"]);

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate("user"));
    }

    [Fact]
    public void Validate_is_case_sensitive_for_strings()
    {
        var validator = new ForbiddenValuesValidator<string>(["admin"]);

        Assert.IsType<StrongTypeValidationResult.Valid>(validator.Validate("ADMIN"));
    }

    [Fact]
    public void Validate_honors_the_comparer_carried_by_the_set()
    {
        var validator = new ForbiddenValuesValidator<string>(ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase, "admin"));

        Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("ADMIN"));
    }

    [Fact]
    public void Invalid_result_carries_an_error_message_with_the_forbidden_values()
    {
        var validator = new ForbiddenValuesValidator<string>(["admin"]);

        var result = Assert.IsType<StrongTypeValidationResult.Invalid>(validator.Validate("admin"));
        Assert.Equal("Value must not be one of: admin.", result.ErrorMessage);
    }
}
