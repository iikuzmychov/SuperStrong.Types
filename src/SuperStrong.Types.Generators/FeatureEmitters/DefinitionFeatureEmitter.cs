using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class DefinitionFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => !model.UserDeclaresDefinition;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        var definitionTypeName = $"{SuperStrong_Types_StrongTypeDefinition}<{model.PrimitiveTypeName}>";
        var strongTypeInterface = $"{SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>";

        using (writer.Block($"partial class {model.TypeName}"))
        {
            var body = model.TemplateTypeName is not null
                ? $"{SuperStrong_Types_StrongType}.GetTemplateDefinition<{model.TemplateTypeName}, {model.PrimitiveTypeName}>()"
                : $"{SuperStrong_Types_StrongType}.Define<{model.PrimitiveTypeName}>()";

            writer.MemberLine($"public static {definitionTypeName} Definition {{ get; }} = {body};");
            writer.Line();
            writer.MemberLine($"static {definitionTypeName} {strongTypeInterface}.Definition => Definition;");
        }
    }
}
