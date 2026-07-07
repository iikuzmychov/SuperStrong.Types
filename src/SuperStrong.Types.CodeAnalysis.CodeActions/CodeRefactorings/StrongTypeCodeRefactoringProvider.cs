using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using System.Collections.Immutable;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

[ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(StrongTypeCodeRefactoringProvider))]
internal sealed class StrongTypeCodeRefactoringProvider : CodeRefactoringProvider
{
    private static readonly CodeRefactoringProvider[] _providers =
    [
        new ImplementDefineCodeRefactoringProvider(),
        new GenerateEqualityCodeRefactoringProvider(),
        new GenerateToStringCodeRefactoringProvider(),
    ];

    public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
    {
        var actions = ImmutableArray.CreateBuilder<CodeAction>();

        foreach (var provider in _providers)
        {
            var nestedContext = new CodeRefactoringContext(
                context.Document,
                context.Span,
                actions.Add,
                context.CancellationToken);

            await provider.ComputeRefactoringsAsync(nestedContext).ConfigureAwait(false);
        }

        foreach (var action in actions)
        {
            context.RegisterRefactoring(action);
        }
    }

    protected override CodeActionRequestPriority ComputeRequestPriority() => CodeActionRequestPriority.High;
}
