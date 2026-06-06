using Microsoft.CodeAnalysis;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal abstract class LiftedFeatureEmitter : OptionalFeatureEmitter<LiftedFeatureState>
{
    public abstract string TargetInterfaceMetadataName { get; }

    public override LiftedFeatureState ResolveState(
        INamedTypeSymbol typeSymbol,
        ITypeSymbol primitiveTypeSymbol,
        INamedTypeSymbol? templateSymbol,
        Compilation compilation)
    {
        var isEnabled = ResolveIsEnabled(typeSymbol, templateSymbol, compilation);

        var openInterfaceSymbol = compilation.GetTypeByMetadataName(TargetInterfaceMetadataName);
        var targetInterfaceSymbol = openInterfaceSymbol?.Construct(typeSymbol);
        var sourceInterfaceSymbol = openInterfaceSymbol?.Construct(primitiveTypeSymbol);

        var userImplements =
            targetInterfaceSymbol is not null &&
            typeSymbol.AllInterfaces.Any(@interface => SymbolEqualityComparer.Default.Equals(@interface, targetInterfaceSymbol));

        var primitiveSupports =
            sourceInterfaceSymbol is not null &&
            primitiveTypeSymbol.AllInterfaces.Any(@interface => SymbolEqualityComparer.Default.Equals(@interface, sourceInterfaceSymbol));

        return new LiftedFeatureState(FeatureName, isEnabled, userImplements, primitiveSupports);
    }

    protected override bool ShouldEmit(LiftedFeatureState state)
    {
        return !state.UserImplements && state.PrimitiveSupports;
    }
}
