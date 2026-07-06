using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal sealed class DefinitionFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => !model.UserDeclaresDefine;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        var definitionTypeName = $"{SuperStrong_Types_StrongTypeDefinition}<{model.PrimitiveTypeName}>";
        var strongTypeInterface = $"{SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>";

        var body = model.TemplateTypeName is not null
            ? $"{SuperStrong_Types_StrongType}.GetTemplateDefinition<{model.TemplateTypeName}, {model.PrimitiveTypeName}>()"
            : $"{SuperStrong_Types_StrongType}.Define<{model.PrimitiveTypeName}>()";

        using (writer.Block($"partial class {model.TypeName}"))
        {
            writer.MemberLine($"static {definitionTypeName} {strongTypeInterface}.Define() => {body};");
        }
    }
}
