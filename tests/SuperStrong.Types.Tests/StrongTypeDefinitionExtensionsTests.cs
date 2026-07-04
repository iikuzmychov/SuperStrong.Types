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
        Assert.Null(validator.ErrorMessageFactory);
    }

    [Fact]
    public void Satisfies_passes_the_error_message_factory_to_the_validator()
    {
        Func<int, string?> errorMessageFactory = value => $"Bad value {value}.";

        var definition = StrongType.Define<int>().Satisfies(value => value > 0, errorMessageFactory);

        var validator = Assert.IsType<PredicateValidator<int>>(Assert.Single(definition.Validators));
        Assert.Same(errorMessageFactory, validator.ErrorMessageFactory);
    }

    [Fact]
    public void Satisfies_wraps_the_error_message_in_a_factory()
    {
        var definition = StrongType.Define<int>().Satisfies(value => value > 0, "Value is not positive.");

        var validator = Assert.IsType<PredicateValidator<int>>(Assert.Single(definition.Validators));
        Assert.NotNull(validator.ErrorMessageFactory);
        Assert.Equal("Value is not positive.", validator.ErrorMessageFactory(0));
    }

    [Fact]
    public void IsNotEmpty_adds_NotEmptyValidator()
    {
        var definition = StrongType.Define<string>().IsNotEmpty();

        var validator = Assert.IsType<NotEmptyValidator>(Assert.Single(definition.Validators));
        Assert.False(validator.AllowWhiteSpaces);
    }

    [Fact]
    public void IsNotEmpty_passes_allowWhiteSpaces_to_the_validator()
    {
        var definition = StrongType.Define<string>().IsNotEmpty(allowWhiteSpaces: true);

        var validator = Assert.IsType<NotEmptyValidator>(Assert.Single(definition.Validators));
        Assert.True(validator.AllowWhiteSpaces);
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
    public void HasLength_adds_ExactLengthValidator()
    {
        var definition = StrongType.Define<string>().HasLength(4);

        var validator = Assert.IsType<ExactLengthValidator>(Assert.Single(definition.Validators));
        Assert.Equal(4, validator.Length);
    }

    [Fact]
    public void IsPositive_adds_PositiveValidator()
    {
        var definition = StrongType.Define<int>().IsPositive();

        var validator = Assert.IsType<PositiveValidator<int>>(Assert.Single(definition.Validators));
        Assert.False(validator.AllowZero);
    }

    [Fact]
    public void IsNegative_adds_NegativeValidator()
    {
        var definition = StrongType.Define<int>().IsNegative();

        var validator = Assert.IsType<NegativeValidator<int>>(Assert.Single(definition.Validators));
        Assert.False(validator.AllowZero);
    }

    [Fact]
    public void IsPositiveOrZero_adds_PositiveValidator_allowing_zero()
    {
        var definition = StrongType.Define<int>().IsPositiveOrZero();

        var validator = Assert.IsType<PositiveValidator<int>>(Assert.Single(definition.Validators));
        Assert.True(validator.AllowZero);
    }

    [Fact]
    public void IsNegativeOrZero_adds_NegativeValidator_allowing_zero()
    {
        var definition = StrongType.Define<int>().IsNegativeOrZero();

        var validator = Assert.IsType<NegativeValidator<int>>(Assert.Single(definition.Validators));
        Assert.True(validator.AllowZero);
    }
}
