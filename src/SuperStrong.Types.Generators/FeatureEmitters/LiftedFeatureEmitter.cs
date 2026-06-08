using Microsoft.CodeAnalysis;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal abstract class LiftedFeatureEmitter : OptionalFeatureEmitter<LiftedFeatureState>
{
    public abstract string TargetInterfaceMetadataName { get; }

    protected virtual ITypeSymbol[] GetTypeArguments(ITypeSymbol primarySymbol, Compilation compilation) => [primarySymbol];

    public override LiftedFeatureState ResolveState(
        INamedTypeSymbol typeSymbol,
        ITypeSymbol primitiveTypeSymbol,
        INamedTypeSymbol? templateSymbol,
        Compilation compilation)
    {
        var openInterfaceSymbol = compilation.GetTypeByMetadataName(TargetInterfaceMetadataName);

        INamedTypeSymbol? sourceInterfaceSymbol;

        if (openInterfaceSymbol is { IsGenericType: true })
        {
            sourceInterfaceSymbol = openInterfaceSymbol.Construct(GetTypeArguments(primitiveTypeSymbol, compilation));
        }
        else
        {
            sourceInterfaceSymbol = openInterfaceSymbol;
        }

        var primitiveSupports =
            sourceInterfaceSymbol is not null &&
            primitiveTypeSymbol.AllInterfaces.Any(@interface => SymbolEqualityComparer.Default.Equals(@interface, sourceInterfaceSymbol));

        return new LiftedFeatureState(FeatureName, primitiveSupports);
    }

    protected override bool ShouldEmit(LiftedFeatureState state)
    {
        return state.PrimitiveSupports;
    }
}
