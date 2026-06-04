using SuperStrong.Types.Generators.Constants;
using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class StrongTypeInterfaceFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => true;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {TypeNames.IStrongType}<{model.TypeName}, {model.PrimitiveType}>"))
        {
            using (writer.Block($"public static {model.TypeName} Create({model.PrimitiveType} value)"))
            {
                writer.Line($"{TypeNames.StrongType}.EnsureValid(value, Definition);");
                writer.Line();
                writer.Line($"return new {model.TypeName}(value);");
            }
            writer.Line();

            writer.Line($"public {model.PrimitiveType} AsPrimitive() => _value;");
        }
    }
}
