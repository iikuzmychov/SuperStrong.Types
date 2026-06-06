using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class FormattableFeatureEmitter : LiftedFeatureEmitter
{
    public override string FeatureAttributeMetadataName => SuperStrong_Types_StrongTypeFeatures_Lifting_FormattableAttribute.MetadataName();
    public override string TargetInterfaceMetadataName => System_IFormattable.MetadataName();
    public override bool IsEnabledByDefault => true;

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_IFormattable}"))
        {
            using (writer.Block($"public string ToString(string? format, {System_IFormatProvider}? formatProvider)"))
            {
                writer.Line($"return InvokeToString<{model.PrimitiveTypeName}>(_value, format, formatProvider);");
                writer.Line();

                using (writer.Block($"static string InvokeToString<T>(T value, string? format, {System_IFormatProvider}? formatProvider) where T : {System_IFormattable}"))
                {
                    writer.Line("return value.ToString(format, formatProvider);");
                }
            }
        }
    }
}
