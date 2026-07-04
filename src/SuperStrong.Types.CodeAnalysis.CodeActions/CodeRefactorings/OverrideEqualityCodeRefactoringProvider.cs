using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperStrong.Types.CodeAnalysis.Shared;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

internal sealed class OverrideEqualityCodeRefactoringProvider : CodeRefactoringProvider
{
    public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
    {
        if (await CodeActionHelpers.TryResolveStrongTypeAsync(context) is not { } target)
        {
            return;
        }

        if (StrongTypeDetection.DeclaresEquals(target.Symbol) &&
            StrongTypeDetection.DeclaresGetHashCode(target.Symbol))
        {
            return;
        }

        context.RegisterRefactoring(
            CodeAction.Create(
                "Override Equals(T) and GetHashCode()",
                cancellationToken => AddEqualityAsync(
                    context.Document,
                    target.ClassDeclaration,
                    target.Symbol,
                    cancellationToken),
                equivalenceKey: nameof(OverrideEqualityCodeRefactoringProvider),
                priority: CodeActionPriority.High));
    }

    internal static Task<Document> AddEqualityAsync(
        Document document,
        ClassDeclarationSyntax classDeclaration,
        INamedTypeSymbol strongTypeSymbol,
        CancellationToken cancellationToken)
    {
        var memberDeclarations = new List<MemberDeclarationSyntax>();

        if (!StrongTypeDetection.DeclaresEquals(strongTypeSymbol))
        {
            var equalsDeclaration = SyntaxFactory.ParseMemberDeclaration(
                $"public bool Equals({strongTypeSymbol.Name}? other) => other is not null && _value.Equals(other._value);")!;

            memberDeclarations.Add(equalsDeclaration.WithElasticSpacing());
        }

        if (!StrongTypeDetection.DeclaresGetHashCode(strongTypeSymbol))
        {
            var getHashCodeDeclaration = SyntaxFactory.ParseMemberDeclaration(
                "public override int GetHashCode() => _value.GetHashCode();")!;

            memberDeclarations.Add(getHashCodeDeclaration.WithElasticSpacing());
        }

        var newClassDeclaration = classDeclaration
            .EnsureBlockBody()
            .AddMembers([.. memberDeclarations]);

        return CodeActionHelpers.ReplaceClassAndFormatAsync(
            document,
            classDeclaration,
            newClassDeclaration,
            cancellationToken);
    }
}
