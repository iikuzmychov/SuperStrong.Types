using Microsoft.CodeAnalysis;
using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

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

        return FeatureAttributeResolver.ResolveIsEnabled(
            featureAttributeSymbol,
            IsEnabledByDefault,
            typeSymbol,
            templateSymbol,
            compilation);
    }
}
