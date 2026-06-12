using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperStrong.Types.CodeAnalysis.Shared;
using System.Collections.Immutable;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefinitionPropertyCodeFixProvider))]
internal sealed class DefinitionPropertyCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => [StrongTypeDiagnostics.DefinitionProperty.Id];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();

        var property = root
            .FindToken(diagnostic.Location.SourceSpan.Start)
            .Parent?
            .FirstAncestorOrSelf<PropertyDeclarationSyntax>();

        if (property?.ExpressionBody is null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                "Use get-only Definition property",
                cancellationToken => ConvertToGetOnlyPropertyAsync(context.Document, property, cancellationToken),
                equivalenceKey: nameof(DefinitionPropertyCodeFixProvider)),
            diagnostic);
    }

    private static async Task<Document> ConvertToGetOnlyPropertyAsync(
        Document document,
        PropertyDeclarationSyntax property,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        if (root is null || property.ExpressionBody is null)
        {
            return document;
        }

        var accessor = SyntaxFactory
            .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

        var accessorList = SyntaxFactory.AccessorList(SyntaxFactory.SingletonList(accessor));

        var updatedProperty = property
            .WithAccessorList(accessorList)
            .WithExpressionBody(null)
            .WithInitializer(SyntaxFactory.EqualsValueClause(property.ExpressionBody.Expression))
            .WithSemicolonToken(property.SemicolonToken);

        var newRoot = root.ReplaceNode(property, updatedProperty);

        return document.WithSyntaxRoot(newRoot);
    }
}
