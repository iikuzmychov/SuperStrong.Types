using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class Utf8SpanFormattableFeatureEmitter : LiftedFeatureEmitter
{
    public override string FeatureAttributeMetadataName => SuperStrong_Types_StrongTypeFeatures_Lifting_Utf8SpanFormattableAttribute.MetadataName();
    public override string TargetInterfaceMetadataName => System_IUtf8SpanFormattable.MetadataName();
    public override bool IsEnabledByDefault => true;

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_IUtf8SpanFormattable}"))
        {
            using (writer.Block($"public bool TryFormat({System_Span}<byte> utf8Destination, out int bytesWritten, {System_ReadOnlySpan}<char> format, {System_IFormatProvider}? provider)"))
            {
                writer.Line($"return InvokeTryFormat<{model.PrimitiveTypeName}>(_value, utf8Destination, out bytesWritten, format, provider);");
                writer.Line();

                using (writer.Block($"static bool InvokeTryFormat<T>(T value, {System_Span}<byte> utf8Destination, out int bytesWritten, {System_ReadOnlySpan}<char> format, {System_IFormatProvider}? provider) where T : {System_IUtf8SpanFormattable}"))
                {
                    writer.Line("return value.TryFormat(utf8Destination, out bytesWritten, format, provider);");
                }
            }
        }
    }
}
