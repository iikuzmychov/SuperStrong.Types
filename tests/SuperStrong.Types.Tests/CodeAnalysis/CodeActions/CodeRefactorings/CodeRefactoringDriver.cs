using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperStrong.Types.CodeAnalysis.Generators;

namespace SuperStrong.Types.Tests.CodeAnalysis.CodeActions.CodeRefactorings;

internal static class CodeRefactoringDriver
{
    public static async Task<string> ApplyAsync(CodeRefactoringProvider refactoring, string source)
    {
        var (document, classDeclaration) = CreateContext(source);
        var actions = await ComputeActionsAsync(refactoring, document, classDeclaration);

        if (actions.Count == 0)
        {
            return source;
        }

        var operations = await actions[0].GetOperationsAsync(CancellationToken.None);
        var applyChanges = operations.OfType<ApplyChangesOperation>().Single();
        var changedDocument = applyChanges.ChangedSolution.GetDocument(document.Id)!;
        var changedText = await changedDocument.GetTextAsync();

        return changedText.ToString();
    }

    public static async Task<bool> OffersRefactoringAsync(CodeRefactoringProvider refactoring, string source)
    {
        var (document, classDeclaration) = CreateContext(source);
        var actions = await ComputeActionsAsync(refactoring, document, classDeclaration);

        return actions.Count > 0;
    }

    public static async Task<IReadOnlyList<CodeAction>> GetActionsAsync(CodeRefactoringProvider refactoring, string source)
    {
        var (document, classDeclaration) = CreateContext(source);

        return await ComputeActionsAsync(refactoring, document, classDeclaration);
    }

    private static async Task<List<CodeAction>> ComputeActionsAsync(
        CodeRefactoringProvider refactoring,
        Document document,
        ClassDeclarationSyntax classDeclaration)
    {
        var actions = new List<CodeAction>();

        var context = new CodeRefactoringContext(
            document,
            classDeclaration.Identifier.Span,
            registerRefactoring: actions.Add,
            CancellationToken.None);

        await refactoring.ComputeRefactoringsAsync(context);

        return actions;
    }

    private static (Document Document, ClassDeclarationSyntax ClassDeclaration) CreateContext(string source)
    {
        var document = TestWorkspace.CreateDocument(new StrongTypeGenerator(), source);
        var root = document.GetSyntaxRootAsync().GetAwaiter().GetResult()!;

        var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

        var classDeclaration = classes.FirstOrDefault(c => c.AttributeLists
                .SelectMany(list => list.Attributes)
                .Any(attribute => attribute.Name.ToString().Contains("StrongType")))
            ?? classes.First();

        return (document, classDeclaration);
    }
}
