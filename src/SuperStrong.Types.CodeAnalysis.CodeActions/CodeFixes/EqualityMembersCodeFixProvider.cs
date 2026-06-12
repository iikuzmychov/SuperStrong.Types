using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperStrong.Types.CodeAnalysis.Shared;
using System.Collections.Immutable;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EqualityMembersCodeFixProvider))]
internal sealed class EqualityMembersCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => [StrongTypeDiagnostics.EqualityMembers.Id];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();

        var classDeclaration = root
            .FindToken(diagnostic.Location.SourceSpan.Start)
            .Parent?
            .FirstAncestorOrSelf<ClassDeclarationSyntax>();

        if (classDeclaration is null)
        {
            return;
        }

        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

        if (semanticModel is null ||
            semanticModel.GetDeclaredSymbol(classDeclaration, context.CancellationToken) is not INamedTypeSymbol strongTypeSymbol)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                "Override Equals(T) and GetHashCode()",
                cancellationToken => OverrideEqualityCodeRefactoringProvider.AddEqualityAsync(
                    context.Document,
                    classDeclaration,
                    strongTypeSymbol,
                    cancellationToken),
                equivalenceKey: nameof(EqualityMembersCodeFixProvider)),
            diagnostic);
    }
}
