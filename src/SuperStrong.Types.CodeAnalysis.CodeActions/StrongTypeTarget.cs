using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

internal record StrongTypeTarget(
    ClassDeclarationSyntax ClassDeclaration,
    INamedTypeSymbol Symbol,
    ITypeSymbol PrimitiveSymbol);
