using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class HasStrongTypeDefinitionFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => !model.UserImplementsDefinition;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        var hasDefinitionInterfaceTypeName = $"{SuperStrong_Types_IHasStrongTypeDefinition}<{model.PrimitiveTypeName}>";
        var definitionTypeName = $"{SuperStrong_Types_StrongTypeDefinition}<{model.PrimitiveTypeName}>";

        using (writer.Block($"partial class {model.TypeName} : {hasDefinitionInterfaceTypeName}"))
        {
            var body = model.TemplateTypeName is not null
                ? $"{SuperStrong_Types_StrongType}.GetTemplateDefinition<{model.TemplateTypeName}, {model.PrimitiveTypeName}>()"
                : $"{SuperStrong_Types_StrongType}.Define<{model.PrimitiveTypeName}>()";

            writer.Line($"public static {definitionTypeName} Definition {{ get; }} = {body};");
            writer.Line();
            writer.Line($"static {definitionTypeName} {hasDefinitionInterfaceTypeName}.Definition => Definition;");
        }
    }
}
