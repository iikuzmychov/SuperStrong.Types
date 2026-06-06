using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class SpanFormattableFeatureEmitter : LiftedFeatureEmitter
{
    public override string FeatureAttributeMetadataName => SuperStrong_Types_StrongTypeSpanFormattableFeatureAttribute.MetadataName();
    public override string TargetInterfaceMetadataName => System_ISpanFormattable.MetadataName();
    public override bool IsEnabledByDefault => true;

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_ISpanFormattable}"))
        {
            using (writer.Block($"public bool TryFormat({System_Span}<char> destination, out int charsWritten, {System_ReadOnlySpan}<char> format, {System_IFormatProvider}? provider)"))
            {
                writer.Line($"return InvokeTryFormat<{model.PrimitiveTypeName}>(_value, destination, out charsWritten, format, provider);");
                writer.Line();

                using (writer.Block($"static bool InvokeTryFormat<T>(T value, {System_Span}<char> destination, out int charsWritten, {System_ReadOnlySpan}<char> format, {System_IFormatProvider}? provider) where T : {System_ISpanFormattable}"))
                {
                    writer.Line("return value.TryFormat(destination, out charsWritten, format, provider);");
                }
            }
        }
    }
}
