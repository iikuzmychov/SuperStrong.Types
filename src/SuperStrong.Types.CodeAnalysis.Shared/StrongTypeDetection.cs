using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SuperStrong.Types.CodeAnalysis.Shared;

internal static class StrongTypeDetection
{
    private const string StrongTypeAttributeName = "StrongTypeAttribute";
    private const string SuperStrongTypesNamespace = "SuperStrong.Types";
    private const string GeneratedCodeAttributeMetadataName = "System.CodeDom.Compiler.GeneratedCodeAttribute";
    private const string StrongTypeDefinitionTypeName = "StrongTypeDefinition";
    private const string DefinitionPropertyName = "Definition";

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

    public static bool IsDefinitionProperty(IPropertySymbol property)
    {
        return
            property.IsStatic &&
            property.Name == DefinitionPropertyName &&
            property.Type is INamedTypeSymbol propertyType &&
            propertyType.Name == StrongTypeDefinitionTypeName &&
            propertyType.ContainingNamespace.ToDisplayString() == SuperStrongTypesNamespace;
    }

    public static bool DeclaresDefinition(INamedTypeSymbol typeSymbol)
    {
        return typeSymbol
            .GetMembers(DefinitionPropertyName)
            .OfType<IPropertySymbol>()
            .Where(IsUserDeclared)
            .Any(property => property.IsStatic);
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
