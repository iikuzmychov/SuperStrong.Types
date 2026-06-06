using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class CoreFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => true;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>"))
        {
            writer.Line($"private readonly {model.PrimitiveTypeName} _value;");
            writer.Line();

            using (writer.Block($"private {model.TypeName}({model.PrimitiveTypeName} value)"))
            {
                writer.Line("_value = value;");
            }

            writer.Line();

            using (writer.Block($"public static {model.TypeName} Create({model.PrimitiveTypeName} value)"))
            {
                writer.Line($"{SuperStrong_Types_StrongType}.EnsureValid(value, Definition);");
                writer.Line();
                writer.Line($"return new {model.TypeName}(value);");
            }

            writer.Line();

            writer.Line($"public {model.PrimitiveTypeName} AsPrimitive() => _value;");
        }
    }
}
