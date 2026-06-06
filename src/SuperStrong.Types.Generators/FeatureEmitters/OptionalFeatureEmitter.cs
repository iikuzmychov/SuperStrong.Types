using Microsoft.CodeAnalysis;
using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;
using System.Collections.Immutable;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal abstract class OptionalFeatureEmitter<TState> : IOptionalFeatureEmitter
    where TState : OptionalFeatureState
{
    public string FeatureName => GetType().Name;

    public abstract string FeatureAttributeMetadataName { get; }

    public abstract bool IsEnabledByDefault { get; }

    public abstract TState ResolveState(
        INamedTypeSymbol typeSymbol,
        ITypeSymbol primitiveTypeSymbol,
        INamedTypeSymbol? templateSymbol,
        Compilation compilation);

    OptionalFeatureState IOptionalFeatureEmitter.ResolveState(
        INamedTypeSymbol typeSymbol,
        ITypeSymbol primitiveTypeSymbol,
        INamedTypeSymbol? templateSymbol,
        Compilation compilation)
        => ResolveState(typeSymbol, primitiveTypeSymbol, templateSymbol, compilation);

    bool IStrongTypeFeatureEmitter.ShouldEmit(StrongTypeModel model)
    {
        var state = (TState)model.OptionalFeatures.First(s => s.FeatureName == FeatureName);
        return state.IsEnabled && ShouldEmit(state);
    }

    protected virtual bool ShouldEmit(TState state) => true;

    public abstract void Emit(IndentedWriter writer, StrongTypeModel model);

    protected bool ResolveIsEnabled(
        INamedTypeSymbol typeSymbol,
        INamedTypeSymbol? templateSymbol,
        Compilation compilation)
    {
        var featureAttributeSymbol = compilation.GetTypeByMetadataName(FeatureAttributeMetadataName);

        if (featureAttributeSymbol is null)
        {
            return IsEnabledByDefault;
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

        return IsEnabledByDefault;
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
