using SuperStrong.Types.Generators.Constants;
using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class HasStrongTypeDefinitionFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => !model.UserImplementsDefinition;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        var hasDefinitionInterfaceTypeName = $"{TypeNames.IHasStrongTypeDefinition}<{model.PrimitiveType}>";
        var definitionTypeName = $"{TypeNames.StrongTypeDefinition}<{model.PrimitiveType}>";

        using (writer.Block($"partial class {model.TypeName} : {hasDefinitionInterfaceTypeName}"))
        {
            var body = model.TemplateType is not null
                ? $"{TypeNames.StrongType}.GetTemplateDefinition<{model.TemplateType}, {model.PrimitiveType}>()"
                : $"{TypeNames.StrongType}.Define<{model.PrimitiveType}>()";

            writer.Line($"public static {definitionTypeName} Definition => {body};");
            writer.Line();
            writer.Line($"static {definitionTypeName} {hasDefinitionInterfaceTypeName}.Definition => Definition;");
        }
    }
}
