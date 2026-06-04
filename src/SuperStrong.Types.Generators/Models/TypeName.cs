namespace SuperStrong.Types.Generators.Models;

internal sealed record TypeName(string? Namespace, string Name)
{
    public string FullyQualifiedName => string.IsNullOrEmpty(Namespace) ? Name : $"global::{Namespace}.{Name}";

    public override string ToString() => FullyQualifiedName;
}
