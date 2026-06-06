using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SuperStrong.Types.Generators.Constants;
using SuperStrong.Types.Generators.FeatureEmitters;
using SuperStrong.Types.Generators.Models;
using System.Collections.Immutable;
using System.Reflection.Metadata;
using System.Text;

namespace SuperStrong.Types.Generators;

[Generator]
internal sealed class StrongTypeGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor ConflictingAttributesDescriptor = new(
        id: "SST001",
        title: "Conflicting StrongType attributes",
        messageFormat: "Type '{0}' is annotated with both [StrongType<TPrimitive>] and [StrongType<TPrimitive, TTemplate>]. Use only one.",
        category: "SuperStrong",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor NotPartialDescriptor = new(
        id: "SST002",
        title: "Strong type must be partial",
        messageFormat: "Type '{0}' is annotated with [StrongType<...>] but is not declared partial. Add the 'partial' modifier so the generator can extend it.",
        category: "SuperStrong",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor RecordDeclarationDescriptor = new(
        id: "SST004",
        title: "Strong type cannot be a record",
        messageFormat: "Type '{0}' is annotated with [StrongType<...>] but is declared as a record. Declare it as a class instead.",
        category: "SuperStrong",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor HasBaseTypeDescriptor = new(
        id: "SST005",
        title: "Strong type cannot inherit from another type",
        messageFormat: "Type '{0}' is annotated with [StrongType<...>] but inherits from '{1}'. Remove the base type.",
        category: "SuperStrong",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

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
                if (isStrongTypeAttributeName(attribute.Name))
                {
                    return true;
                }
            }
        }

        return false;

        static bool isStrongTypeAttributeName(NameSyntax name)
        {
            var simpleName = name switch
            {
                QualifiedNameSyntax qualified => qualified.Right.Identifier.ValueText,
                SimpleNameSyntax simple => simple.Identifier.ValueText,
                _ => null,
            };

            if (simpleName is null)
            {
                return false;
            }

            return
                simpleName == TypeNames.StrongTypeAttribute.Name ||
                simpleName + nameof(Attribute) == TypeNames.StrongTypeAttribute.Name;
        }
    }

    private static GeneratorOutput? BuildOutput(GeneratorSyntaxContext context)
    {
        if (context.SemanticModel.GetDeclaredSymbol(context.Node) is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        var strongTypeAttributes = typeSymbol
            .GetAttributes()
            .Where(isStrongTypeAttribute)
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

        static bool isStrongTypeAttribute(AttributeData attribute)
        {
            return
                attribute.AttributeClass is { } attributeClass &&
                attributeClass.Name == TypeNames.StrongTypeAttribute.Name &&
                attributeClass.ContainingNamespace.ToDisplayString() == Namespaces.SuperStrong_Types;
        }
    }

    private static StrongTypeModel BuildModel(INamedTypeSymbol typeSymbol, AttributeData attribute, Compilation compilation)
    {
        var typeArguments = attribute.AttributeClass!.TypeArguments;
        var primitiveTypeSymbol = typeArguments[0];
        var primitiveType = primitiveTypeSymbol.ToDisplayString(FullyQualifiedFormatWithKeywords);

        var templateType = typeArguments.Length == 2
            ? typeArguments[1].ToDisplayString(FullyQualifiedFormatWithKeywords)
            : null;

        var ancestors = ImmutableArray.CreateBuilder<string>();

        for (var current = typeSymbol.ContainingType; current is not null; current = current.ContainingType)
        {
            ancestors.Add(current.Name);
        }

        ancestors.Reverse();

        var namespaceName = typeSymbol.ContainingNamespace.IsGlobalNamespace
            ? null
            : typeSymbol.ContainingNamespace.ToDisplayString();

        var hasDefinitionInterface = compilation
            .GetTypeByMetadataName(TypeNames.IHasStrongTypeDefinition.MetadataName(arity: 1))
            ?.Construct(primitiveTypeSymbol);

        var userImplementsDefinition =
            hasDefinitionInterface is not null &&
            typeSymbol.AllInterfaces.Any(
                @interface => SymbolEqualityComparer.Default.Equals(@interface, hasDefinitionInterface));

        return new StrongTypeModel(
            Namespace: namespaceName,
            TypeName: typeSymbol.Name,
            AncestorTypeNames: ancestors.ToImmutable(),
            PrimitiveType: primitiveType,
            TemplateType: templateType,
            UserImplementsDefinition: userImplementsDefinition);
    }

    private static void Emit(SourceProductionContext context, GeneratorOutput output)
    {
        output.Switch(
            onModel: model =>
            {
                var featues = ImmutableArray.Create<IStrongTypeFeatureEmitter>(
                [
                    new CoreFeatureEmitter(),
                    new HasStrongTypeDefinitionFeatureEmitter(),
                    new StrongTypeInterfaceFeatureEmitter(),
                    new EqualityFeatureEmitter(),
                ]);

                var source = SourceBuilder.Build(model, featues);
                context.AddSource(model.HintName, SourceText.From(source, Encoding.UTF8));
            },
            onConflict: conflict =>
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    ConflictingAttributesDescriptor,
                        location: conflict.Location?.ToLocation() ?? Location.None,
                        messageArgs: conflict.TypeFullName));
            },
            onNotPartial: notPartial =>
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    NotPartialDescriptor,
                        location: notPartial.Location?.ToLocation() ?? Location.None,
                        messageArgs: notPartial.TypeFullName));
            },
            onRecord: record =>
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    RecordDeclarationDescriptor,
                        location: record.Location?.ToLocation() ?? Location.None,
                        messageArgs: record.TypeFullName));
            },
            onHasBaseType: hasBaseType =>
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    HasBaseTypeDescriptor,
                        location: hasBaseType.Location?.ToLocation() ?? Location.None,
                        messageArgs: [hasBaseType.TypeFullName, hasBaseType.BaseTypeFullName]));
            });
    }
}
