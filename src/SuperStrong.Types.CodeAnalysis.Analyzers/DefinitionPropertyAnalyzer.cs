using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SuperStrong.Types.CodeAnalysis.Shared;
using System.Collections.Immutable;

namespace SuperStrong.Types.CodeAnalysis.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class DefinitionPropertyAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [StrongTypeDiagnostics.DefinitionProperty];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeProperty, SyntaxKind.PropertyDeclaration);
    }

    private static void AnalyzeProperty(SyntaxNodeAnalysisContext context)
    {
        var property = (PropertyDeclarationSyntax)context.Node;

        if (property.ExpressionBody is { } expressionBody &&
            property.Parent is ClassDeclarationSyntax classDeclaration &&
            context.SemanticModel.GetDeclaredSymbol(property) is IPropertySymbol propertySymbol &&
            StrongTypeDetection.IsDefinitionProperty(propertySymbol) &&
            StrongTypeDetection.GetStrongTypeSymbolInfo(classDeclaration, context.SemanticModel) is not null)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    StrongTypeDiagnostics.DefinitionProperty,
                    expressionBody.ArrowToken.GetLocation()));
        }
    }
}
