using SuperStrong.Types.Validators;
using System.Text.RegularExpressions;

namespace SuperStrong.Types.Tests;

public sealed class StrongTypeDefinitionExtensionsTests
{
    [Fact]
    public void Satisfies_adds_PredicateValidator()
    {
        Func<int, bool> predicate = value => value > 0;

        var definition = StrongType.Define<int>().Satisfies(predicate);

        var validator = Assert.IsType<PredicateValidator<int>>(Assert.Single(definition.Validators));
        Assert.Same(predicate, validator.Predicate);
    }

    [Fact]
    public void IsNotWhiteSpace_adds_NotWhiteSpaceValidator()
    {
        var definition = StrongType.Define<string>().IsNotWhiteSpace();

        Assert.IsType<NotWhiteSpaceValidator>(Assert.Single(definition.Validators));
    }

    [Fact]
    public void IsLowerInvariant_adds_LowerInvariantValidator()
    {
        var definition = StrongType.Define<string>().IsLowerInvariant();

        Assert.IsType<LowerInvariantValidator>(Assert.Single(definition.Validators));
    }

    [Fact]
    public void IsUpperInvariant_adds_UpperInvariantValidator()
    {
        var definition = StrongType.Define<string>().IsUpperInvariant();

        Assert.IsType<UpperInvariantValidator>(Assert.Single(definition.Validators));
    }

    [Fact]
    public void MatchesRegex_adds_RegexValidator_with_pattern()
    {
        var definition = StrongType.Define<string>().MatchesRegex("^a$");

        var validator = Assert.IsType<RegexValidator>(Assert.Single(definition.Validators));
        Assert.Equal("^a$", validator.Regex.ToString());
    }

    [Fact]
    public void MatchesRegex_passes_options_to_the_validator()
    {
        var definition = StrongType.Define<string>().MatchesRegex("^a$", RegexOptions.IgnoreCase);

        var validator = Assert.IsType<RegexValidator>(Assert.Single(definition.Validators));
        Assert.Equal("^a$", validator.Regex.ToString());
        Assert.Equal(RegexOptions.IgnoreCase, validator.Regex.Options);
    }

    [Fact]
    public void MatchesRegex_accepts_a_regex()
    {
        var regex = new Regex("^a$");

        var definition = StrongType.Define<string>().MatchesRegex(regex);

        var validator = Assert.IsType<RegexValidator>(Assert.Single(definition.Validators));
        Assert.Same(regex, validator.Regex);
    }

    [Fact]
    public void IsOneOf_adds_AllowedValuesValidator()
    {
        var definition = StrongType.Define<int>().IsOneOf(1, 2, 3);

        var validator = Assert.IsType<AllowedValuesValidator<int>>(Assert.Single(definition.Validators));
        Assert.True(validator.AllowedValues.SetEquals([1, 2, 3]));
    }

    [Fact]
    public void IsNotOneOf_adds_ForbiddenValuesValidator()
    {
        var definition = StrongType.Define<int>().IsNotOneOf(1, 2, 3);

        var validator = Assert.IsType<ForbiddenValuesValidator<int>>(Assert.Single(definition.Validators));
        Assert.True(validator.ForbiddenValues.SetEquals([1, 2, 3]));
    }

    [Fact]
    public void IsNot_adds_a_ForbiddenValuesValidator_with_the_single_value()
    {
        var definition = StrongType.Define<int>().IsNot(0);

        var validator = Assert.IsType<ForbiddenValuesValidator<int>>(Assert.Single(definition.Validators));
        Assert.True(validator.ForbiddenValues.SetEquals([0]));
    }

    [Fact]
    public void IsOneOf_passes_the_comparer_to_the_validator()
    {
        var definition = StrongType.Define<string>().IsOneOf(["a", "b"], StringComparer.OrdinalIgnoreCase);

        var validator = Assert.IsType<AllowedValuesValidator<string>>(Assert.Single(definition.Validators));
        Assert.Same(StringComparer.OrdinalIgnoreCase, validator.AllowedValues.KeyComparer);
    }

    [Fact]
    public void IsNotOneOf_passes_the_comparer_to_the_validator()
    {
        var definition = StrongType.Define<string>().IsNotOneOf(["a", "b"], StringComparer.OrdinalIgnoreCase);

        var validator = Assert.IsType<ForbiddenValuesValidator<string>>(Assert.Single(definition.Validators));
        Assert.Same(StringComparer.OrdinalIgnoreCase, validator.ForbiddenValues.KeyComparer);
    }

    [Fact]
    public void HasMinValue_passes_exclusivity_to_the_validator()
    {
        var definition = StrongType.Define<int>().HasMinValue(5, isExclusive: true);

        var validator = Assert.IsType<MinValueValidator<int>>(Assert.Single(definition.Validators));
        Assert.Equal(5, validator.MinValue);
        Assert.True(validator.IsExclusive);
    }

    [Fact]
    public void HasMaxValue_passes_exclusivity_to_the_validator()
    {
        var definition = StrongType.Define<int>().HasMaxValue(5, isExclusive: true);

        var validator = Assert.IsType<MaxValueValidator<int>>(Assert.Single(definition.Validators));
        Assert.Equal(5, validator.MaxValue);
        Assert.True(validator.IsExclusive);
    }

    [Fact]
    public void IsNotEmpty_adds_a_MinLengthValidator_of_one()
    {
        var definition = StrongType.Define<string>().IsNotEmpty();

        var validator = Assert.IsType<MinLengthValidator>(Assert.Single(definition.Validators));
        Assert.Equal(1, validator.MinLength);
    }

    [Fact]
    public void HasLength_adds_matching_MinLengthValidator_and_MaxLengthValidator()
    {
        var definition = StrongType.Define<string>().HasLength(4);

        Assert.Collection(
            definition.Validators,
            validator => Assert.Equal(4, Assert.IsType<MinLengthValidator>(validator).MinLength),
            validator => Assert.Equal(4, Assert.IsType<MaxLengthValidator>(validator).MaxLength));
    }

    [Fact]
    public void IsPositive_adds_an_exclusive_MinValueValidator_of_zero()
    {
        var definition = StrongType.Define<int>().IsPositive();

        var validator = Assert.IsType<MinValueValidator<int>>(Assert.Single(definition.Validators));
        Assert.Equal(0, validator.MinValue);
        Assert.True(validator.IsExclusive);
    }

    [Fact]
    public void IsNegative_adds_an_exclusive_MaxValueValidator_of_zero()
    {
        var definition = StrongType.Define<int>().IsNegative();

        var validator = Assert.IsType<MaxValueValidator<int>>(Assert.Single(definition.Validators));
        Assert.Equal(0, validator.MaxValue);
        Assert.True(validator.IsExclusive);
    }

    [Fact]
    public void IsPositiveOrZero_adds_an_inclusive_MinValueValidator_of_zero()
    {
        var definition = StrongType.Define<int>().IsPositiveOrZero();

        var validator = Assert.IsType<MinValueValidator<int>>(Assert.Single(definition.Validators));
        Assert.Equal(0, validator.MinValue);
        Assert.False(validator.IsExclusive);
    }

    [Fact]
    public void IsNegativeOrZero_adds_an_inclusive_MaxValueValidator_of_zero()
    {
        var definition = StrongType.Define<int>().IsNegativeOrZero();

        var validator = Assert.IsType<MaxValueValidator<int>>(Assert.Single(definition.Validators));
        Assert.Equal(0, validator.MaxValue);
        Assert.False(validator.IsExclusive);
    }
}
