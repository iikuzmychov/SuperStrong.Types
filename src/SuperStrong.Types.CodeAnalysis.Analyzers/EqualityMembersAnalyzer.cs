using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SuperStrong.Types.CodeAnalysis.Shared;
using System.Collections.Immutable;

namespace SuperStrong.Types.CodeAnalysis.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class EqualityMembersAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [StrongTypeDiagnostics.EqualityMembers];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeClass(SyntaxNodeAnalysisContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        if (StrongTypeDetection.GetStrongTypeSymbolInfo(classDeclaration, context.SemanticModel) is not { } strongType)
        {
            return;
        }

        var declaresEquals = StrongTypeDetection.DeclaresEquals(strongType.Symbol);
        var declaresGetHashCode = StrongTypeDetection.DeclaresGetHashCode(strongType.Symbol);
        var equalityIsIncomplete = declaresEquals != declaresGetHashCode;

        if (equalityIsIncomplete)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    StrongTypeDiagnostics.EqualityMembers,
                    classDeclaration.Identifier.GetLocation()));
        }
    }
}
