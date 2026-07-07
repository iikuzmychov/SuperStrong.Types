using Microsoft.CodeAnalysis;

namespace SuperStrong.Types.CodeAnalysis.Shared;

internal static class StrongTypeDiagnostics
{
    public const string Category = "SuperStrong";

    public static readonly DiagnosticDescriptor ConflictingAttributes = new(
        id: "SST001",
        title: "Conflicting StrongType attributes",
        messageFormat: "Strong type '{0}' is annotated with both [StrongType<TPrimitive>] and [StrongType<TPrimitive, TTemplate>]",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor NotPartial = new(
        id: "SST002",
        title: "Strong type must be partial",
        messageFormat: "Strong type '{0}' must be partial",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor RecordDeclaration = new(
        id: "SST003",
        title: "Strong type cannot be a record",
        messageFormat: "Strong type '{0}' cannot be a record",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor HasBaseType = new(
        id: "SST004",
        title: "Strong type cannot inherit from another type",
        messageFormat: "Strong type '{0}' cannot inherit from '{1}'",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor EqualityMembers = new(
        id: "SST005",
        title: "Implement Equals and GetHashCode together",
        messageFormat: "Implement Equals(T) and GetHashCode() together",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor AbstractDeclaration = new(
        id: "SST006",
        title: "Strong type cannot be abstract",
        messageFormat: "Strong type '{0}' cannot be abstract",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}
