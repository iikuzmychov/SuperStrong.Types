using SuperStrong.Types.Generators.Helpers;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class CoreFeatureEmitter : IStrongTypeFeatureEmitter
{
    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName}"))
        {
            writer.Line($"private readonly {model.PrimitiveType} _value;");
            writer.Line();

            using (writer.Block($"private {model.TypeName}({model.PrimitiveType} value)"))
            {
                writer.Line("_value = value;");
            }
        }
    }
}
