using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SuperStrong.Types.CodeAnalysis.Shared;
using SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;
using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;
using System.Collections.Immutable;
using System.Text;

namespace SuperStrong.Types.CodeAnalysis.Generators;

[Generator]
internal sealed class StrongTypeGenerator : IIncrementalGenerator
{
    private static readonly SymbolDisplayFormat FullyQualifiedFormatWithKeywords =
        SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
            SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions | SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var outputs = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => IsCandidate(node),
                transform: static (syntaxContext, _) => BuildOutput(syntaxContext))
            .Where(static output => output is not null);

        context.RegisterSourceOutput(outputs, Emit!);
    }

    private static bool IsCandidate(SyntaxNode node)
    {
        if (node is not (ClassDeclarationSyntax or RecordDeclarationSyntax))
        {
            return false;
        }

        var typeDeclaration = (TypeDeclarationSyntax)node;

        foreach (var attributeList in typeDeclaration.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                if (StrongTypeDetection.IsStrongTypeAttributeName(attribute.Name))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static GeneratorOutput? BuildOutput(GeneratorSyntaxContext context)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        var strongTypeAttributes = typeSymbol
            .GetAttributes()
            .Where(StrongTypeDetection.IsStrongTypeAttribute)
            .ToList();

        if (strongTypeAttributes.Count == 0)
        {
            return null;
        }

        if (strongTypeAttributes.Count > 1)
        {
            return GeneratorOutput.FromConflict(
                new ConflictingAttributesInfo(
                    TypeFullName: typeSymbol.ToDisplayString(),
                    Location: LocationInfo.From(context.Node)));
        }

        if (context.Node is RecordDeclarationSyntax)
        {
            return GeneratorOutput.FromRecord(
                new RecordDeclarationInfo(
                    TypeFullName: typeSymbol.ToDisplayString(),
                    Location: LocationInfo.From(context.Node)));
        }

        var typeDeclaration = (TypeDeclarationSyntax)context.Node;
        var isPartial = typeDeclaration.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PartialKeyword));

        if (!isPartial)
        {
            return GeneratorOutput.FromNotPartial(
                new NotPartialInfo(
                    TypeFullName: typeSymbol.ToDisplayString(),
                    Location: LocationInfo.From(context.Node)));
        }

        if (typeSymbol.BaseType is { SpecialType: not SpecialType.System_Object } baseType)
        {
            return GeneratorOutput.FromHasBaseType(
                new HasBaseTypeInfo(
                    TypeFullName: typeSymbol.ToDisplayString(),
                    BaseTypeFullName: baseType.ToDisplayString(),
                    Location: LocationInfo.From(context.Node)));
        }

        var model = BuildModel(typeSymbol, strongTypeAttributes.Single(), context.SemanticModel.Compilation);

        return GeneratorOutput.FromModel(model);

    }

    private static StrongTypeModel BuildModel(INamedTypeSymbol typeSymbol, AttributeData attribute, Compilation compilation)
    {
        var typeArguments = attribute.AttributeClass!.TypeArguments;
        var primitiveTypeSymbol = typeArguments[0];
        var primitiveType = primitiveTypeSymbol.ToDisplayString(FullyQualifiedFormatWithKeywords);

        var templateTypeSymbol = typeArguments.Length == 2
            ? typeArguments[1] as INamedTypeSymbol
            : null;

        var templateType = templateTypeSymbol?.ToDisplayString(FullyQualifiedFormatWithKeywords);

        var ancestors = ImmutableArray.CreateBuilder<AncestorInfo>();

        for (var current = typeSymbol.ContainingType; current is not null; current = current.ContainingType)
        {
            ancestors.Add(new AncestorInfo(current.Name, GetAncestorDeclarationKeyword(current)));
        }

        ancestors.Reverse();

        var namespaceName = typeSymbol.ContainingNamespace.IsGlobalNamespace
            ? null
            : typeSymbol.ContainingNamespace.ToDisplayString();

        var userDeclaresDefinition = StrongTypeDetection.DeclaresDefinition(typeSymbol);
        var userOverridesToString = StrongTypeDetection.DeclaresToString(typeSymbol);
        var userOverridesEquals = StrongTypeDetection.DeclaresEquals(typeSymbol);
        var userOverridesGetHashCode = StrongTypeDetection.DeclaresGetHashCode(typeSymbol);

        var optionalFeatures = FeatureRegistry.Optional
            .Select(emitter => emitter.ResolveState(typeSymbol, primitiveTypeSymbol, templateTypeSymbol, compilation))
            .ToImmutableArray();

        return new StrongTypeModel
        {
            Namespace = namespaceName,
            TypeName = typeSymbol.Name,
            Ancestors = ancestors.ToImmutable(),
            PrimitiveTypeName = primitiveType,
            TemplateTypeName = templateType,
            UserDeclaresDefinition = userDeclaresDefinition,
            UserOverridesToString = userOverridesToString,
            UserOverridesEquals = userOverridesEquals,
            UserOverridesGetHashCode = userOverridesGetHashCode,
            OptionalFeatures = optionalFeatures,
        };
    }

    private static string GetAncestorDeclarationKeyword(INamedTypeSymbol symbol)
    {
        return symbol.TypeKind switch
        {
            TypeKind.Struct => "struct",
            TypeKind.Class => "class",
            _ => throw new InvalidOperationException($"Unsupported type kind '{symbol.TypeKind}' for ancestor '{symbol.Name}'."),
        };
    }

    private static void Emit(SourceProductionContext context, GeneratorOutput output)
    {
        output.Switch(
            onModel: model =>
            {
                var activeFeatures = FeatureRegistry.All
                    .Where(feature => feature.ShouldEmit(model))
                    .ToImmutableArray();

                var source = SourceBuilder.Build(model, activeFeatures);
                context.AddSource(model.HintName, SourceText.From(source, Encoding.UTF8));
            },
            onConflict: conflict =>
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        StrongTypeDiagnostics.ConflictingAttributes,
                        location: conflict.Location?.ToLocation(),
                        messageArgs: conflict.TypeFullName));
            },
            onNotPartial: notPartial =>
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        StrongTypeDiagnostics.NotPartial,
                        location: notPartial.Location?.ToLocation(),
                        messageArgs: notPartial.TypeFullName));
            },
            onRecord: record =>
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        StrongTypeDiagnostics.RecordDeclaration,
                        location: record.Location?.ToLocation(),
                        messageArgs: record.TypeFullName));
            },
            onHasBaseType: hasBaseType =>
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        StrongTypeDiagnostics.HasBaseType,
                        location: hasBaseType.Location?.ToLocation(),
                        messageArgs: [hasBaseType.TypeFullName, hasBaseType.BaseTypeFullName]));
            });
    }
}
