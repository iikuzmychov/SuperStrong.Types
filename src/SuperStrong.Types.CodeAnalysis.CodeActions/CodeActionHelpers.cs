using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;
using SuperStrong.Types.CodeAnalysis.Shared;

namespace SuperStrong.Types.CodeAnalysis.CodeActions;

internal static class CodeActionHelpers
{
    public static async Task<StrongTypeTarget?> TryResolveStrongTypeAsync(CodeRefactoringContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var classDeclaration = root?.FindNode(context.Span).FirstAncestorOrSelf<ClassDeclarationSyntax>();

        if (classDeclaration is null)
        {
            return null;
        }

        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

        if (semanticModel is null ||
            StrongTypeDetection.GetStrongTypeSymbolInfo(classDeclaration, semanticModel) is not { } strongType)
        {
            return null;
        }

        return new StrongTypeTarget(classDeclaration, strongType.StrongTypeSymbol, strongType.PrimitiveTypeSymbol);
    }

    public static ClassDeclarationSyntax EnsureBlockBody(this ClassDeclarationSyntax classDecl)
    {
        if (!classDecl.SemicolonToken.IsKind(SyntaxKind.SemicolonToken))
        {
            return classDecl;
        }

        return classDecl
            .WithSemicolonToken(default)
            .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
            .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken));
    }


    public static MemberDeclarationSyntax WithElasticSpacing(this MemberDeclarationSyntax member)
    {
        return member
            .WithLeadingTrivia(SyntaxFactory.ElasticMarker)
            .WithTrailingTrivia(SyntaxFactory.ElasticMarker);
    }

    public static ClassDeclarationSyntax InsertMembers(
        this ClassDeclarationSyntax classDeclaration,
        int index,
        IReadOnlyList<MemberDeclarationSyntax> members)
    {
        if (members.Count == 0)
        {
            return classDeclaration;
        }

        return classDeclaration.WithMembers(classDeclaration.Members.InsertRange(index, members));
    }

    public static async Task<Document> ReplaceClassAndFormatAsync(
        Document document,
        ClassDeclarationSyntax originalClass,
        ClassDeclarationSyntax newClass,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        if (root is null)
        {
            return document;
        }

        var annotatedClass = newClass.WithAdditionalAnnotations(Formatter.Annotation, Simplifier.Annotation);
        var newRoot = root.ReplaceNode(originalClass, annotatedClass);

        return document.WithSyntaxRoot(newRoot);
    }

    public static int GetDefinitionInsertionIndex(this ClassDeclarationSyntax classDeclaration)
    {
        Func<MemberDeclarationSyntax, bool>[] anchorsByPriority =
        [
            static member => member is PropertyDeclarationSyntax property && IsPublic(property) && IsStatic(property),
            static member => member is PropertyDeclarationSyntax property && IsPublic(property),
            static member => member is PropertyDeclarationSyntax,
            static member => member is FieldDeclarationSyntax or EventFieldDeclarationSyntax,
        ];

        foreach (var isAnchor in anchorsByPriority)
        {
            var anchorIndex = LastMemberIndex(classDeclaration, isAnchor);

            if (anchorIndex >= 0)
            {
                return anchorIndex + 1;
            }
        }

        return 0;
    }

    private static int LastMemberIndex(ClassDeclarationSyntax classDeclaration, Func<MemberDeclarationSyntax, bool> predicate)
    {
        var index = -1;

        for (var i = 0; i < classDeclaration.Members.Count; i++)
        {
            if (predicate(classDeclaration.Members[i]))
            {
                index = i;
            }
        }

        return index;
    }

    private static bool IsPublic(PropertyDeclarationSyntax property)
    {
        return property.Modifiers.Any(SyntaxKind.PublicKeyword);
    }

    private static bool IsStatic(PropertyDeclarationSyntax property)
    {
        return property.Modifiers.Any(SyntaxKind.StaticKeyword);
    }
}
