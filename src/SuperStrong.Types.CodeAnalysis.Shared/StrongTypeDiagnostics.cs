using Microsoft.CodeAnalysis;

namespace SuperStrong.Types.CodeAnalysis.Shared;

internal static class StrongTypeDiagnostics
{
    public const string Category = "SuperStrong";

    public static readonly DiagnosticDescriptor ConflictingAttributes = new(
        id: "SST001",
        title: "Conflicting StrongType attributes",
        messageFormat: "Type '{0}' is annotated with both [StrongType<TPrimitive>] and [StrongType<TPrimitive, TTemplate>]. Use only one.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor NotPartial = new(
        id: "SST002",
        title: "Strong type must be partial",
        messageFormat: "Type '{0}' is annotated with [StrongType<...>] but is not declared partial. Add the 'partial' modifier so the generator can extend it.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor RecordDeclaration = new(
        id: "SST003",
        title: "Strong type cannot be a record",
        messageFormat: "Type '{0}' is annotated with [StrongType<...>] but is declared as a record. Declare it as a class instead.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor HasBaseType = new(
        id: "SST004",
        title: "Strong type cannot inherit from another type",
        messageFormat: "Type '{0}' is annotated with [StrongType<...>] but inherits from '{1}'. Remove the base type.",
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
}
