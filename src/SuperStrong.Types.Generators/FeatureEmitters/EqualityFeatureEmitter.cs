using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class EqualityFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => true;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        var blockHeader = model.UserImplementsIEquatable
            ? $"partial class {model.TypeName}"
            : $"partial class {model.TypeName} : global::System.IEquatable<{model.TypeName}>";

        using (writer.Block(blockHeader))
        {
            if (!model.UserImplementsIEquatable)
            {
                writer.Line($"public bool Equals({model.TypeName}? other) => other is not null && _value.Equals(other._value);");
                writer.Line();
            }

            writer.Line($"public override bool Equals(object? obj) => obj is {model.TypeName} other && Equals(other);");
            writer.Line();

            if (model.UserImplementsIEquatable)
            {
                writer.Line("public override partial int GetHashCode();");
            }
            else
            {
                writer.Line("public override int GetHashCode() => _value.GetHashCode();");
            }
        }
    }
}
