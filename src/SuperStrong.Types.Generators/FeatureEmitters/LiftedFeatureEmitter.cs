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
        var isEnabled = ResolveIsEnabled(typeSymbol, templateSymbol, compilation);

        var openInterfaceSymbol = compilation.GetTypeByMetadataName(TargetInterfaceMetadataName);

        INamedTypeSymbol? targetInterfaceSymbol;
        INamedTypeSymbol? sourceInterfaceSymbol;

        if (openInterfaceSymbol is { IsGenericType: true })
        {
            targetInterfaceSymbol = openInterfaceSymbol.Construct(GetTypeArguments(typeSymbol, compilation));
            sourceInterfaceSymbol = openInterfaceSymbol.Construct(GetTypeArguments(primitiveTypeSymbol, compilation));
        }
        else
        {
            targetInterfaceSymbol = openInterfaceSymbol;
            sourceInterfaceSymbol = openInterfaceSymbol;
        }

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
