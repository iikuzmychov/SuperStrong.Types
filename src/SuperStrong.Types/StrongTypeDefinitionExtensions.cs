using SuperStrong.Types.Validators;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

namespace SuperStrong.Types;

public static class StrongTypeDefinitionExtensions
{
    public static StrongTypeDefinition<TPrimitive> Satisfies<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        Func<TPrimitive, bool> predicate,
        Func<TPrimitive, string?>? errorMessageFactory = null)
        where TPrimitive : notnull
    {
        return definition.WithValidator(new PredicateValidator<TPrimitive>(predicate, errorMessageFactory));
    }

    public static StrongTypeDefinition<TPrimitive> Satisfies<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        Func<TPrimitive, bool> predicate,
        string? errorMessage = null)
        where TPrimitive : notnull
    {
        return definition.Satisfies(predicate, _ => errorMessage);
    }

    public static StrongTypeDefinition<TPrimitive> Satisfies<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        Func<TPrimitive, bool> predicate)
        where TPrimitive : notnull
    {
        return definition.Satisfies(predicate, errorMessageFactory: null);
    }

    public static StrongTypeDefinition<TPrimitive> IsOneOf<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        params ImmutableArray<TPrimitive> allowedValues)
        where TPrimitive : notnull
    {
        return definition.WithValidator(new AllowedValuesValidator<TPrimitive>(allowedValues.ToImmutableHashSet()));
    }

    public static StrongTypeDefinition<TPrimitive> IsOneOf<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        ImmutableArray<TPrimitive> allowedValues,
        IEqualityComparer<TPrimitive> comparer)
        where TPrimitive : notnull
    {
        return definition.WithValidator(new AllowedValuesValidator<TPrimitive>(allowedValues.ToImmutableHashSet(comparer)));
    }

    public static StrongTypeDefinition<TPrimitive> IsNotOneOf<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        params ImmutableArray<TPrimitive> forbiddenValues)
        where TPrimitive : notnull
    {
        return definition.WithValidator(new ForbiddenValuesValidator<TPrimitive>(forbiddenValues.ToImmutableHashSet()));
    }

    public static StrongTypeDefinition<TPrimitive> IsNotOneOf<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        ImmutableArray<TPrimitive> forbiddenValues,
        IEqualityComparer<TPrimitive> comparer)
        where TPrimitive : notnull
    {
        return definition.WithValidator(new ForbiddenValuesValidator<TPrimitive>(forbiddenValues.ToImmutableHashSet(comparer)));
    }

    public static StrongTypeDefinition<TPrimitive> IsNot<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        TPrimitive value)
        where TPrimitive : notnull
    {
        return definition.IsNotOneOf(value);
    }

    public static StrongTypeDefinition<TPrimitive> HasMaxValue<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        TPrimitive maxValue,
        bool isExclusive = false)
        where TPrimitive : IComparable<TPrimitive>
    {
        return definition.WithValidator(new MaxValueValidator<TPrimitive>(maxValue, isExclusive));
    }

    public static StrongTypeDefinition<TPrimitive> HasMinValue<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition,
        TPrimitive minValue,
        bool isExclusive = false)
        where TPrimitive : IComparable<TPrimitive>
    {
        return definition.WithValidator(new MinValueValidator<TPrimitive>(minValue, isExclusive));
    }

    public static StrongTypeDefinition<TPrimitive> IsPositive<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition)
        where TPrimitive : INumberBase<TPrimitive>, IComparable<TPrimitive>
    {
        return definition.WithValidator(new PositiveValidator<TPrimitive>());
    }

    public static StrongTypeDefinition<TPrimitive> IsNegative<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition)
        where TPrimitive : INumberBase<TPrimitive>, IComparable<TPrimitive>
    {
        return definition.WithValidator(new NegativeValidator<TPrimitive>());
    }

    public static StrongTypeDefinition<TPrimitive> IsPositiveOrZero<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition)
        where TPrimitive : INumberBase<TPrimitive>, IComparable<TPrimitive>
    {
        return definition.WithValidator(new PositiveValidator<TPrimitive>(allowZero: true));
    }

    public static StrongTypeDefinition<TPrimitive> IsNegativeOrZero<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> definition)
        where TPrimitive : INumberBase<TPrimitive>, IComparable<TPrimitive>
    {
        return definition.WithValidator(new NegativeValidator<TPrimitive>(allowZero: true));
    }

    public static StrongTypeDefinition<string> HasMaxLength(
        this StrongTypeDefinition<string> definition,
        int maxLength)
    {
        return definition.WithValidator(new MaxLengthValidator(maxLength));
    }

    public static StrongTypeDefinition<string> HasMinLength(
        this StrongTypeDefinition<string> definition,
        int minLength)
    {
        return definition.WithValidator(new MinLengthValidator(minLength));
    }

    public static StrongTypeDefinition<string> HasLength(
        this StrongTypeDefinition<string> definition,
        int length)
    {
        return definition.WithValidator(new ExactLengthValidator(length));
    }

    public static StrongTypeDefinition<string> IsNotEmpty(
        this StrongTypeDefinition<string> definition,
        bool allowWhiteSpaces = false)
    {
        return definition.WithValidator(new NotEmptyValidator(allowWhiteSpaces));
    }

    public static StrongTypeDefinition<string> MatchesRegex(
        this StrongTypeDefinition<string> definition,
        [StringSyntax(StringSyntaxAttribute.Regex)] string pattern,
        RegexOptions options = RegexOptions.None)
    {
        return definition.WithValidator(new RegexValidator(new Regex(pattern, options)));
    }

    public static StrongTypeDefinition<string> MatchesRegex(
        this StrongTypeDefinition<string> definition,
        Regex regex)
    {
        return definition.WithValidator(new RegexValidator(regex));
    }

    public static StrongTypeDefinition<string> IsLowerInvariant(this StrongTypeDefinition<string> definition)
    {
        return definition.WithValidator(new LowerInvariantValidator());
    }

    public static StrongTypeDefinition<string> IsUpperInvariant(this StrongTypeDefinition<string> definition)
    {
        return definition.WithValidator(new UpperInvariantValidator());
    }
}
