using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class Utf8SpanParsableFeatureEmitter : LiftedFeatureEmitter
{
    public override string FeatureAttributeMetadataName => SuperStrong_Types_StrongTypeFeatures_Lifting_Utf8SpanParsableAttribute.MetadataName();
    public override string TargetInterfaceMetadataName => System_IUtf8SpanParsable.MetadataName(arity: 1);
    public override bool IsEnabledByDefault => true;

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_IUtf8SpanParsable}<{model.TypeName}>"))
        {
            using (writer.Block($"public static {model.TypeName} Parse({System_ReadOnlySpan}<byte> utf8Text, {System_IFormatProvider}? provider)"))
            {
                writer.Line($"return Create(InvokeParse<{model.PrimitiveTypeName}>(utf8Text, provider));");
                writer.Line();

                using (writer.Block($"static T InvokeParse<T>({System_ReadOnlySpan}<byte> utf8Text, {System_IFormatProvider}? provider) where T : {System_IUtf8SpanParsable}<T>"))
                {
                    writer.Line("return T.Parse(utf8Text, provider);");
                }
            }

            writer.Line();

            using (writer.Block($"public static bool TryParse({System_ReadOnlySpan}<byte> utf8Text, {System_IFormatProvider}? provider, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                using (writer.Block($"if (InvokeTryParse<{model.PrimitiveTypeName}>(utf8Text, provider, out var primitive) && {SuperStrong_Types_StrongType}.IsValid(primitive, Definition))"))
                {
                    writer.Line($"result = new {model.TypeName}(primitive);");
                    writer.Line("return true;");
                }

                writer.Line();
                writer.Line("result = null;");
                writer.Line("return false;");
                writer.Line();

                using (writer.Block($"static bool InvokeTryParse<T>({System_ReadOnlySpan}<byte> utf8Text, {System_IFormatProvider}? provider, out T result) where T : {System_IUtf8SpanParsable}<T>"))
                {
                    writer.Line("return T.TryParse(utf8Text, provider, out result!);");
                }
            }
        }
    }
}
