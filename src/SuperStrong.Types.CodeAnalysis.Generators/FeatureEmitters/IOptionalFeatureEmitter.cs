using Microsoft.CodeAnalysis;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal interface IOptionalFeatureEmitter : IStrongTypeFeatureEmitter
{
    public string FeatureName { get; }

    public OptionalFeatureState ResolveState(
        INamedTypeSymbol typeSymbol,
        ITypeSymbol primitiveTypeSymbol,
        INamedTypeSymbol? templateSymbol,
        Compilation compilation);
}
