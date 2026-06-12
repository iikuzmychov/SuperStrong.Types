using System.Collections.Immutable;
using System.Text;

namespace SuperStrong.Types.CodeAnalysis.Generators.Models;

internal sealed record StrongTypeModel
{
    public required string? Namespace { get; init; }
    public required string TypeName { get; init; }
    public required ImmutableArray<AncestorInfo> Ancestors { get; init; }
    public required string PrimitiveTypeName { get; init; }
    public required string? TemplateTypeName { get; init; }
    public required bool UserDeclaresDefinition { get; init; }
    public required bool UserOverridesToString { get; init; }
    public required bool UserOverridesEquals { get; init; }
    public required bool UserOverridesGetHashCode { get; init; }
    public required ImmutableArray<OptionalFeatureState> OptionalFeatures { get; init; }

    public string HintName
    {
        get
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrEmpty(Namespace))
            {
                builder.Append(Namespace!.Replace('.', '_'));
                builder.Append('_');
            }

            foreach (var ancestor in Ancestors)
            {
                builder.Append(ancestor.Name);
                builder.Append('_');
            }

            builder.Append(TypeName);
            builder.Append(".g.cs");

            return builder.ToString();
        }
    }
}
