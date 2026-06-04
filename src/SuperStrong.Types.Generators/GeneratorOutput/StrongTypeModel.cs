using System.Collections.Immutable;
using System.Text;

namespace SuperStrong.Types.Generators;

internal sealed record StrongTypeModel(
    string? Namespace,
    string TypeName,
    ImmutableArray<string> AncestorTypeNames,
    string PrimitiveType,
    string? TemplateType)
{
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

            foreach (var ancestor in AncestorTypeNames)
            {
                builder.Append(ancestor);
                builder.Append('_');
            }

            builder.Append(TypeName);
            builder.Append(".g.cs");

            return builder.ToString();
        }
    }
}
