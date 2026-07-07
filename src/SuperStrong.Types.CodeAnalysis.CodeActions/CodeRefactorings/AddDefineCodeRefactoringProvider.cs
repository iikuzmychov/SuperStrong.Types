using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperStrong.Types.CodeAnalysis.Shared;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

internal sealed class AddDefineCodeRefactoringProvider : CodeRefactoringProvider
{
    private static readonly SymbolDisplayFormat _primitiveDisplayFormat =
        SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
            SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions | SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
    {
        if (await CodeActionHelpers.TryResolveStrongTypeAsync(context) is not { } target)
        {
            return;
        }

        if (StrongTypeDetection.DeclaresDefine(target.Symbol, target.PrimitiveSymbol))
        {
            return;
        }

        var primitiveTypeName = target.PrimitiveSymbol.ToDisplayString(_primitiveDisplayFormat);

        context.RegisterRefactoring(
            CodeAction.Create(
                "Add Define()",
                cancellationToken => AddDefineAsync(
                    context.Document,
                    target.ClassDeclaration,
                    primitiveTypeName,
                    cancellationToken),
                equivalenceKey: nameof(AddDefineCodeRefactoringProvider),
                priority: CodeActionPriority.High));
    }

    private static Task<Document> AddDefineAsync(
        Document document,
        ClassDeclarationSyntax classDeclaration,
        string primitiveTypeName,
        CancellationToken cancellationToken)
    {
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(
            $"public static partial global::SuperStrong.Types.StrongTypeDefinition<{primitiveTypeName}> Define() " +
            $"=> global::SuperStrong.Types.StrongType.Define<{primitiveTypeName}>();")!;

        var blockClassDeclaration = classDeclaration.EnsureBlockBody();
        var insertionIndex = blockClassDeclaration.GetDefineInsertionIndex();

        var newClassDeclaration = blockClassDeclaration.InsertMembers(
            insertionIndex,
            [memberDeclaration.WithElasticSpacing()]);

        return CodeActionHelpers.ReplaceClassAndFormatAsync(
            document,
            classDeclaration,
            newClassDeclaration,
            cancellationToken);
    }
}
