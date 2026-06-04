using SuperStrong.Types.Generators.Constants;
using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class HasStrongTypeLayoutFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => !model.UserImplementsLayout;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        var interfaceTypeName = $"{TypeNames.IHasStrongTypeLayout}<{model.PrimitiveType}>";
        var layoutTypeName = $"{TypeNames.StrongTypeLayout}<{model.PrimitiveType}>";

        using (writer.Block($"partial class {model.TypeName} : {interfaceTypeName}"))
        {
            var body = model.TemplateType is not null
                ? $"{model.TemplateType}.Layout"
                : $"{TypeNames.StrongType}.Layout<{model.PrimitiveType}>()";

            writer.Line($"public static {layoutTypeName} Layout => {body};");
            writer.Line();
            writer.Line($"static {layoutTypeName} {interfaceTypeName}.Layout => Layout;");
        }
    }
}
