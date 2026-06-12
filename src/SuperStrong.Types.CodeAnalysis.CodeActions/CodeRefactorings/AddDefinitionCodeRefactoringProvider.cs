using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperStrong.Types.CodeAnalysis.Shared;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

internal sealed class AddDefinitionCodeRefactoringProvider : CodeRefactoringProvider
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

        if (StrongTypeDetection.DeclaresDefinition(target.StrongTypeSymbol))
        {
            return;
        }

        var primitiveTypeName = target.PrimitiveTypeSymbol.ToDisplayString(_primitiveDisplayFormat);

        context.RegisterRefactoring(
            CodeAction.Create(
                "Add Definition",
                cancellationToken => AddDefinitionAsync(
                    context.Document,
                    target.ClassDeclaration,
                    primitiveTypeName,
                    cancellationToken),
                equivalenceKey: nameof(AddDefinitionCodeRefactoringProvider),
                priority: CodeActionPriority.High));
    }

    private static Task<Document> AddDefinitionAsync(
        Document document,
        ClassDeclarationSyntax classDeclaration,
        string primitiveTypeName,
        CancellationToken cancellationToken)
    {
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(
            $"public static global::SuperStrong.Types.StrongTypeDefinition<{primitiveTypeName}> Definition {{ get; }} " +
            $"= global::SuperStrong.Types.StrongType.Define<{primitiveTypeName}>();")!;

        var blockClassDeclaration = classDeclaration.EnsureBlockBody();
        var insertionIndex = blockClassDeclaration.GetDefinitionInsertionIndex();
        
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
