using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperStrong.Types.CodeAnalysis.Shared;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

internal sealed class OverrideToStringCodeRefactoringProvider : CodeRefactoringProvider
{
    public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
    {
        if (await CodeActionHelpers.TryResolveStrongTypeAsync(context) is not { } target)
        {
            return;
        }

        if (StrongTypeDetection.DeclaresToString(target.StrongTypeSymbol))
        {
            return;
        }

        context.RegisterRefactoring(
            CodeAction.Create(
                "Override ToString()",
                cancellationToken => AddToStringAsync(context.Document, target.ClassDeclaration, cancellationToken),
                equivalenceKey: nameof(OverrideToStringCodeRefactoringProvider),
                priority: CodeActionPriority.High));
    }

    private static Task<Document> AddToStringAsync(
        Document document,
        ClassDeclarationSyntax classDeclaration,
        CancellationToken cancellationToken)
    {
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(
            "public override string ToString() => _value.ToString();")!;

        var newClassDeclaration = classDeclaration
            .EnsureBlockBody()
            .AddMembers(memberDeclaration.WithElasticSpacing());

        return CodeActionHelpers.ReplaceClassAndFormatAsync(
            document,
            classDeclaration,
            newClassDeclaration,
            cancellationToken);
    }
}
