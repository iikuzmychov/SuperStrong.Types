using Microsoft.CodeAnalysis;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal interface IOptionalFeatureEmitter : IStrongTypeFeatureEmitter
{
    public string FeatureName { get; }

    public OptionalFeatureState ResolveState(
        INamedTypeSymbol typeSymbol,
        ITypeSymbol primitiveTypeSymbol,
        INamedTypeSymbol? templateSymbol,
        Compilation compilation);
}
