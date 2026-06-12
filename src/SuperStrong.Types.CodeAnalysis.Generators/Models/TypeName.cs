namespace SuperStrong.Types.CodeAnalysis.Generators.Models;

internal sealed record TypeName(string? Namespace, string Name)
{
    public string FullyQualifiedName => string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";
    public string GlobalFullyQualifiedName => $"global::{FullyQualifiedName}";

    public string MetadataName(int arity = 0) => arity == 0 ? FullyQualifiedName : $"{FullyQualifiedName}`{arity}";

    public override string ToString() => GlobalFullyQualifiedName;
}
