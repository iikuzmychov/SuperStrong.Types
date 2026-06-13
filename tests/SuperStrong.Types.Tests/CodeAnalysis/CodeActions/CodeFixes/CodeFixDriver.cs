using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using SuperStrong.Types.CodeAnalysis.Generators;
using SuperStrong.Types.Tests.CodeAnalysis;
using SuperStrong.Types.Tests.CodeAnalysis.Analyzers;

namespace SuperStrong.Types.Tests.CodeAnalysis.CodeActions.CodeFixes;

internal static class CodeFixDriver
{
    public static async Task<string> ApplyAsync(DiagnosticAnalyzer analyzer, CodeFixProvider codeFix, string source)
    {
        var document = TestWorkspace.CreateDocument(new StrongTypeGenerator(), source);
        var diagnostic = (await AnalyzerDriver.GetDiagnosticsAsync(analyzer, document)).Single();

        var actions = new List<CodeAction>();
        var context = new CodeFixContext(
            document,
            diagnostic,
            (action, _) => actions.Add(action),
            CancellationToken.None);

        await codeFix.RegisterCodeFixesAsync(context);

        var operations = await actions.Single().GetOperationsAsync(CancellationToken.None);
        var applyChanges = operations.OfType<ApplyChangesOperation>().Single();
        var changedDocument = applyChanges.ChangedSolution.GetDocument(document.Id)!;
        var changedText = await changedDocument.GetTextAsync();

        return changedText.ToString();
    }
}
