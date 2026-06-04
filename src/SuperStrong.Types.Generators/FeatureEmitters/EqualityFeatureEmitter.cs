using SuperStrong.Types.Generators.Helpers;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class EqualityFeatureEmitter : IStrongTypeFeatureEmitter
{
    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName}"))
        {
            writer.Line("public override int GetHashCode() => _value.GetHashCode();");
            writer.Line();
            writer.Line($"public override bool Equals(object? obj) => obj is {model.TypeName} other && _value.Equals(other._value);");
        }
    }
}
