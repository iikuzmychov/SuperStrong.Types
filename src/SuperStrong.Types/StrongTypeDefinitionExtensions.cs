using SuperStrong.Types.Validators;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

namespace SuperStrong.Types;

public static class StrongTypeDefinitionExtensions
{
    public static StrongTypeDefinition<TPrimitive> Satisfies<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        Func<TPrimitive, bool> predicate)
        where TPrimitive : notnull
    {
        return builder.WithValidator(new PredicateValidator<TPrimitive>(predicate));
    }

    public static StrongTypeDefinition<TPrimitive> IsOneOf<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        params ImmutableArray<TPrimitive> allowedValues)
        where TPrimitive : notnull
    {
        return builder.WithValidator(new AllowedValuesValidator<TPrimitive>(allowedValues.ToImmutableHashSet()));
    }

    public static StrongTypeDefinition<TPrimitive> IsOneOf<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        ImmutableArray<TPrimitive> allowedValues,
        IEqualityComparer<TPrimitive> comparer)
        where TPrimitive : notnull
    {
        return builder.WithValidator(new AllowedValuesValidator<TPrimitive>(allowedValues.ToImmutableHashSet(comparer)));
    }

    public static StrongTypeDefinition<TPrimitive> IsNotOneOf<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        params ImmutableArray<TPrimitive> forbiddenValues)
        where TPrimitive : notnull
    {
        return builder.WithValidator(new ForbiddenValuesValidator<TPrimitive>(forbiddenValues.ToImmutableHashSet()));
    }

    public static StrongTypeDefinition<TPrimitive> IsNotOneOf<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        ImmutableArray<TPrimitive> forbiddenValues,
        IEqualityComparer<TPrimitive> comparer)
        where TPrimitive : notnull
    {
        return builder.WithValidator(new ForbiddenValuesValidator<TPrimitive>(forbiddenValues.ToImmutableHashSet(comparer)));
    }

    public static StrongTypeDefinition<TPrimitive> IsNot<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        TPrimitive value)
        where TPrimitive : notnull
    {
        return builder.IsNotOneOf(value);
    }

    public static StrongTypeDefinition<TPrimitive> HasMaxValue<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        TPrimitive maxValue,
        bool isExclusive = false)
        where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
    {
        return builder.WithValidator(new MaxValueValidator<TPrimitive>(maxValue, isExclusive));
    }

    public static StrongTypeDefinition<TPrimitive> HasMinValue<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        TPrimitive minValue,
        bool isExclusive = false)
        where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
    {
        return builder.WithValidator(new MinValueValidator<TPrimitive>(minValue, isExclusive));
    }

    public static StrongTypeDefinition<TPrimitive> IsPositive<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder)
        where TPrimitive : INumberBase<TPrimitive>, IComparisonOperators<TPrimitive, TPrimitive, bool>
    {
        return builder.HasMinValue(TPrimitive.Zero, isExclusive: true);
    }

    public static StrongTypeDefinition<TPrimitive> IsNegative<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder)
        where TPrimitive : INumberBase<TPrimitive>, IComparisonOperators<TPrimitive, TPrimitive, bool>
    {
        return builder.HasMaxValue(TPrimitive.Zero, isExclusive: true);
    }

    public static StrongTypeDefinition<TPrimitive> IsPositiveOrZero<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder)
        where TPrimitive : INumberBase<TPrimitive>, IComparisonOperators<TPrimitive, TPrimitive, bool>
    {
        return builder.HasMinValue(TPrimitive.Zero);
    }

    public static StrongTypeDefinition<TPrimitive> IsNegativeOrZero<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder)
        where TPrimitive : INumberBase<TPrimitive>, IComparisonOperators<TPrimitive, TPrimitive, bool>
    {
        return builder.HasMaxValue(TPrimitive.Zero);
    }

    public static StrongTypeDefinition<string> HasMaxLength(
        this StrongTypeDefinition<string> builder,
        int maxLength)
    {
        return builder.WithValidator(new MaxLengthValidator(maxLength));
    }

    public static StrongTypeDefinition<string> HasMinLength(
        this StrongTypeDefinition<string> builder,
        int minLength)
    {
        return builder.WithValidator(new MinLengthValidator(minLength));
    }

    public static StrongTypeDefinition<string> HasLength(
        this StrongTypeDefinition<string> builder,
        int length)
    {
        return builder.HasMinLength(length).HasMaxLength(length);
    }

    public static StrongTypeDefinition<string> IsNotEmpty(this StrongTypeDefinition<string> builder)
    {
        return builder.HasMinLength(1);
    }

    public static StrongTypeDefinition<string> IsNotWhiteSpace(this StrongTypeDefinition<string> builder)
    {
        return builder.WithValidator(new NotWhiteSpaceValidator());
    }

    public static StrongTypeDefinition<string> MatchesRegex(
        this StrongTypeDefinition<string> builder,
        [StringSyntax(StringSyntaxAttribute.Regex)] string pattern,
        RegexOptions options = RegexOptions.None)
    {
        return builder.WithValidator(new RegexValidator(new Regex(pattern, options)));
    }

    public static StrongTypeDefinition<string> MatchesRegex(
        this StrongTypeDefinition<string> builder,
        Regex regex)
    {
        return builder.WithValidator(new RegexValidator(regex));
    }

    public static StrongTypeDefinition<string> IsLowerInvariant(this StrongTypeDefinition<string> builder)
    {
        return builder.WithValidator(new LowerInvariantValidator());
    }

    public static StrongTypeDefinition<string> IsUpperInvariant(this StrongTypeDefinition<string> builder)
    {
        return builder.WithValidator(new UpperInvariantValidator());
    }
}
