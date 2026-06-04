using SuperStrong.Types.Generators.Constants;
using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class HasStrongTypeDefinitionFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => !model.UserImplementsDefinition;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        var interfaceTypeName = $"{TypeNames.IHasStrongTypeDefinition.FullyQualifiedName}<{model.PrimitiveType}>";
        var definitionTypeName = $"{TypeNames.StrongTypeDefinition}<{model.PrimitiveType}>";

        using (writer.Block($"partial class {model.TypeName} : {interfaceTypeName}"))
        {
            var body = model.TemplateType is not null
                ? $"{model.TemplateType}.Definition"
                : $"{TypeNames.StrongType}.Define<{model.PrimitiveType}>()";

            writer.Line($"public static {definitionTypeName} Definition => {body};");
            writer.Line();
            writer.Line($"static {definitionTypeName} {interfaceTypeName}.Definition => Definition;");
        }
    }
}
