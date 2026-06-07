using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace SuperStrong.Types.Generators.Helpers;

internal static class FeatureAttributeResolver
{
    public static bool ResolveIsEnabled(
        INamedTypeSymbol? featureAttributeSymbol,
        bool builtInDefault,
        INamedTypeSymbol typeSymbol,
        INamedTypeSymbol? templateSymbol,
        Compilation compilation)
    {
        if (featureAttributeSymbol is null)
        {
            return builtInDefault;
        }

        if (TryReadIsEnabled(typeSymbol.GetAttributes(), featureAttributeSymbol, out var typeValue))
        {
            return typeValue;
        }

        if (templateSymbol is not null &&
            TryReadIsEnabled(templateSymbol.GetAttributes(), featureAttributeSymbol, out var templateValue))
        {
            return templateValue;
        }

        if (TryReadIsEnabled(compilation.Assembly.GetAttributes(), featureAttributeSymbol, out var assemblyValue))
        {
            return assemblyValue;
        }

        return builtInDefault;
    }

    private static bool TryReadIsEnabled(
        ImmutableArray<AttributeData> attributes,
        INamedTypeSymbol featureAttributeSymbol,
        out bool isEnabled)
    {
        foreach (var attribute in attributes)
        {
            if (!InheritsFrom(attribute.AttributeClass, featureAttributeSymbol))
            {
                continue;
            }

            foreach (var named in attribute.NamedArguments)
            {
                if (named.Key == "IsEnabled" && named.Value.Value is bool value)
                {
                    isEnabled = value;
                    return true;
                }
            }
        }

        isEnabled = false;
        return false;
    }

    private static bool InheritsFrom(INamedTypeSymbol? type, INamedTypeSymbol target)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(current, target))
            {
                return true;
            }
        }

        return false;
    }
}
