using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperStrong.Types.CodeAnalysis.Shared;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

internal sealed class ImplementDefineCodeRefactoringProvider : CodeRefactoringProvider
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

        var partialDeclaration =
            $"public static partial global::SuperStrong.Types.StrongTypeDefinition<{primitiveTypeName}> Define() " +
            $"=> global::SuperStrong.Types.StrongType.Define<{primitiveTypeName}>();";

        var explicitDeclaration =
            $"static global::SuperStrong.Types.StrongTypeDefinition<{primitiveTypeName}> " +
            $"global::SuperStrong.Types.IStrongType<{target.Symbol.Name}, {primitiveTypeName}>.Define() " +
            $"=> global::SuperStrong.Types.StrongType.Define<{primitiveTypeName}>();";

        context.RegisterRefactoring(
            CodeAction.Create(
                "Implement Define()",
                cancellationToken => ImplementDefineAsync(
                    context.Document,
                    target.ClassDeclaration,
                    partialDeclaration,
                    cancellationToken),
                equivalenceKey: nameof(ImplementDefineCodeRefactoringProvider),
                priority: CodeActionPriority.High));

        context.RegisterRefactoring(
            CodeAction.Create(
                "Implement Define() explicitly",
                cancellationToken => ImplementDefineAsync(
                    context.Document,
                    target.ClassDeclaration,
                    explicitDeclaration,
                    cancellationToken),
                equivalenceKey: $"{nameof(ImplementDefineCodeRefactoringProvider)}.Explicit",
                priority: CodeActionPriority.High));
    }

    private static Task<Document> ImplementDefineAsync(
        Document document,
        ClassDeclarationSyntax classDeclaration,
        string defineDeclaration,
        CancellationToken cancellationToken)
    {
        var memberDeclaration = SyntaxFactory.ParseMemberDeclaration(defineDeclaration)!;

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
