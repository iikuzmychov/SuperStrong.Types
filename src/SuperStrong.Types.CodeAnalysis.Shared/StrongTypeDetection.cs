using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SuperStrong.Types.CodeAnalysis.Shared;

internal static class StrongTypeDetection
{
    private const string StrongTypeAttributeName = "StrongTypeAttribute";
    private const string SuperStrongTypesNamespace = "SuperStrong.Types";
    private const string GeneratedCodeAttributeMetadataName = "System.CodeDom.Compiler.GeneratedCodeAttribute";
    private const string StrongTypeDefinitionName = "StrongTypeDefinition";
    private const string StrongTypeInterfaceName = "IStrongType";
    private const string DefineMethodName = "Define";

    private static readonly SymbolDisplayFormat _fullyQualifiedWithoutGlobalPrefix =
        SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);

    public static bool IsStrongTypeAttributeName(NameSyntax name)
    {
        var simpleName = name switch
        {
            QualifiedNameSyntax qualified => qualified.Right.Identifier.ValueText,
            SimpleNameSyntax simple => simple.Identifier.ValueText,
            _ => null,
        };

        return
            simpleName == StrongTypeAttributeName ||
            simpleName + nameof(Attribute) == StrongTypeAttributeName;
    }

    public static bool IsStrongTypeAttribute(AttributeData attribute)
    {
        return
            attribute.AttributeClass is { } attributeClass &&
            attributeClass.Name == StrongTypeAttributeName &&
            attributeClass.ContainingNamespace.ToDisplayString() == SuperStrongTypesNamespace;
    }

    public static StrongTypeSymbolInfo? GetStrongTypeSymbolInfo(
        ClassDeclarationSyntax classDeclaration,
        SemanticModel semanticModel)
    {
        if (semanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol declared)
        {
            return null;
        }

        foreach (var attribute in declared.GetAttributes())
        {
            if (!IsStrongTypeAttribute(attribute))
            {
                continue;
            }

            var typeArguments = attribute.AttributeClass!.TypeArguments;

            if (typeArguments.Length < 1)
            {
                continue;
            }

            return new StrongTypeSymbolInfo(declared, typeArguments[0]);
        }

        return null;
    }

    public static bool DeclaresDefine(INamedTypeSymbol typeSymbol, ITypeSymbol primitiveSymbol)
    {
        return DeclaresDefineImplicitly(typeSymbol, primitiveSymbol) || DeclaresDefineExplicitly(typeSymbol);
    }

    private static bool DeclaresDefineImplicitly(INamedTypeSymbol typeSymbol, ITypeSymbol primitiveSymbol)
    {
        return typeSymbol
            .GetMembers(DefineMethodName)
            .OfType<IMethodSymbol>()
            .Where(IsUserDeclared)
            .Any(method =>
                method.IsStatic &&
                method.Parameters.Length == 0 &&
                method.TypeParameters.Length == 0 &&
                method.ReturnType is INamedTypeSymbol returnType &&
                returnType.Name == StrongTypeDefinitionName &&
                returnType.ContainingNamespace.ToDisplayString() == SuperStrongTypesNamespace &&
                returnType.TypeArguments.Length == 1 &&
                SymbolEqualityComparer.Default.Equals(returnType.TypeArguments[0], primitiveSymbol));
    }

    private static bool DeclaresDefineExplicitly(INamedTypeSymbol typeSymbol)
    {
        return typeSymbol
            .GetMembers()
            .OfType<IMethodSymbol>()
            .Where(IsUserDeclared)
            .SelectMany(method => method.ExplicitInterfaceImplementations)
            .Any(implemented =>
                implemented.Name == DefineMethodName &&
                implemented.ContainingType.Name == StrongTypeInterfaceName &&
                implemented.ContainingType.ContainingNamespace.ToDisplayString() == SuperStrongTypesNamespace);
    }

    public static bool DeclaresToString(INamedTypeSymbol typeSymbol)
    {
        return DeclaresInstanceMethod(typeSymbol, nameof(ToString), parameterCount: 0);
    }

    public static bool DeclaresGetHashCode(INamedTypeSymbol typeSymbol)
    {
        return DeclaresInstanceMethod(typeSymbol, nameof(GetHashCode), parameterCount: 0);
    }

    public static bool DeclaresEquals(INamedTypeSymbol typeSymbol)
    {
        return typeSymbol
            .GetMembers(nameof(Equals))
            .OfType<IMethodSymbol>()
            .Where(IsUserDeclared)
            .Any(method =>
                !method.IsStatic &&
                method.Parameters.Length == 1 &&
                SymbolEqualityComparer.Default.Equals(method.Parameters[0].Type, typeSymbol));
    }

    private static bool DeclaresInstanceMethod(INamedTypeSymbol typeSymbol, string name, int parameterCount)
    {
        return typeSymbol
            .GetMembers(name)
            .OfType<IMethodSymbol>()
            .Where(IsUserDeclared)
            .Any(method => !method.IsStatic && method.Parameters.Length == parameterCount);
    }

    private static bool IsUserDeclared(ISymbol symbol)
    {
        return !symbol.GetAttributes().Any(IsGeneratedCodeAttribute);
    }

    private static bool IsGeneratedCodeAttribute(AttributeData attribute)
    {
        var attributeName = attribute.AttributeClass?.ToDisplayString(_fullyQualifiedWithoutGlobalPrefix);

        return attributeName == GeneratedCodeAttributeMetadataName;
    }
}
