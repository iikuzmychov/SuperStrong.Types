using Microsoft.CodeAnalysis;

namespace SuperStrong.Types.CodeAnalysis.Shared;

internal record StrongTypeSymbolInfo(INamedTypeSymbol StrongTypeSymbol, ITypeSymbol PrimitiveTypeSymbol);
