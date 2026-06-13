using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.Tests.Validators;

public sealed class AllowedValuesValidatorTests
{
    [Fact]
    public void Constructor_throws_when_no_values_are_specified()
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
    public void IsValid_returns_true_for_an_allowed_value()
    {
        var validator = new AllowedValuesValidator<string>(["red", "green", "blue"]);

        Assert.True(validator.IsValid("green"));
    }

    [Fact]
    public void IsValid_returns_false_for_a_value_outside_the_set()
    {
        var validator = new AllowedValuesValidator<string>(["red", "green", "blue"]);

        Assert.False(validator.IsValid("yellow"));
    }

    [Fact]
    public void IsValid_is_case_sensitive_for_strings_by_default()
    {
        var validator = new AllowedValuesValidator<string>(["red"]);

        Assert.False(validator.IsValid("RED"));
    }

    [Fact]
    public void IsValid_honors_the_comparer_carried_by_the_set()
    {
        var validator = new AllowedValuesValidator<string>(ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase, "red"));

        Assert.True(validator.IsValid("RED"));
    }

    [Fact]
    public void EnsureValid_throws_for_a_value_outside_the_set()
    {
        var validator = new AllowedValuesValidator<int>([1, 2, 3]);

        Assert.Throws<ArgumentException>(() => validator.EnsureValid(4));
    }
}
