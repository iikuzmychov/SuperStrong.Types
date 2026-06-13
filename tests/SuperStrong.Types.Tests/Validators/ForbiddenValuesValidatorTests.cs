using SuperStrong.Types.Validators;
using System.Collections.Immutable;

namespace SuperStrong.Types.Tests.Validators;

public sealed class ForbiddenValuesValidatorTests
{
    [Fact]
    public void Constructor_throws_when_no_values_are_specified()
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
    public void IsValid_returns_false_for_a_forbidden_value()
    {
        var validator = new ForbiddenValuesValidator<string>(["admin", "root"]);

        Assert.False(validator.IsValid("admin"));
    }

    [Fact]
    public void IsValid_returns_true_for_a_value_outside_the_set()
    {
        var validator = new ForbiddenValuesValidator<string>(["admin", "root"]);

        Assert.True(validator.IsValid("user"));
    }

    [Fact]
    public void IsValid_honors_the_comparer_carried_by_the_set()
    {
        var validator = new ForbiddenValuesValidator<string>(ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase, "admin"));

        Assert.False(validator.IsValid("ADMIN"));
    }

    [Fact]
    public void EnsureValid_throws_for_a_forbidden_value()
    {
        var validator = new ForbiddenValuesValidator<int>([1, 2, 3]);

        Assert.Throws<ArgumentException>(() => validator.EnsureValid(2));
    }
}
