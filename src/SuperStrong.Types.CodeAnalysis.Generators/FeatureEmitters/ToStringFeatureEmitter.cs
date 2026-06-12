using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal sealed class ToStringFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => !model.UserOverridesToString;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName}"))
        {
            writer.MemberLine("public override string ToString() => _value.ToString();");
        }
    }
}
