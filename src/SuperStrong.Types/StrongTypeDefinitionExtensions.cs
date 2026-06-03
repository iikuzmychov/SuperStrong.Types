using SuperStrong.Types.Validators;
using System.Numerics;

namespace SuperStrong.Types;

public static class StrongTypeDefinitionExtensions
{
    public static StrongTypeDefinition<TPrimitive> HasMaxValue<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        TPrimitive maxValue)
        where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
    {
        return builder.WithValidator(new MaxValueValidator<TPrimitive>(maxValue));
    }

    public static StrongTypeDefinition<TPrimitive> HasMinValue<TPrimitive>(
        this StrongTypeDefinition<TPrimitive> builder,
        TPrimitive minValue)
        where TPrimitive : IComparisonOperators<TPrimitive, TPrimitive, bool>
    {
        return builder.WithValidator(new MinValueValidator<TPrimitive>(minValue));
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
}
