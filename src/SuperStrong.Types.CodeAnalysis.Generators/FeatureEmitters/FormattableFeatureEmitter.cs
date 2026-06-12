using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal sealed class FormattableFeatureEmitter : LiftedFeatureEmitter
{
    public override string TargetInterfaceMetadataName => System_IFormattable.MetadataName();

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_IFormattable}"))
        {
            using (writer.MemberBlock($"public string ToString(string? format, {System_IFormatProvider}? formatProvider)"))
            {
                writer.Line($"return InvokeToString<{model.PrimitiveTypeName}>(_value, format, formatProvider);");
                writer.Line();

                using (writer.Block($"static string InvokeToString<T>(T value, string? format, {System_IFormatProvider}? formatProvider) where T : {System_IFormattable}"))
                {
                    writer.Line("return value.ToString(format, formatProvider);");
                }
            }

            writer.Line();
            using (writer.MemberBlock($"string {System_IFormattable}.ToString(string? format, {System_IFormatProvider}? formatProvider)"))
            {
                writer.Line("return ToString(format, formatProvider);");
            }
        }
    }
}
