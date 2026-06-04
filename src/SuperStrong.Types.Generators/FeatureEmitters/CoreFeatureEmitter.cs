using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class CoreFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => true;

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
