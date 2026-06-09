using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class SpanParsableFeatureEmitter : LiftedFeatureEmitter
{
    public override string TargetInterfaceMetadataName => System_ISpanParsable.MetadataName(arity: 1);

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        if (model.PrimitiveTypeName == "string")
        {
            EmitStringSpecialCase(writer, model);
            return;
        }

        using (writer.Block($"partial class {model.TypeName} : {System_ISpanParsable}<{model.TypeName}>"))
        {
            using (writer.Block($"public static {model.TypeName} Parse({System_ReadOnlySpan}<char> s, {System_IFormatProvider}? provider)"))
            {
                writer.Line($"return From(InvokeParse<{model.PrimitiveTypeName}>(s, provider));");
                writer.Line();

                using (writer.Block($"static T InvokeParse<T>({System_ReadOnlySpan}<char> s, {System_IFormatProvider}? provider) where T : {System_ISpanParsable}<T>"))
                {
                    writer.Line("return T.Parse(s, provider);");
                }
            }

            writer.Line();

            using (writer.Block($"static {model.TypeName} {System_ISpanParsable}<{model.TypeName}>.Parse({System_ReadOnlySpan}<char> s, {System_IFormatProvider}? provider)"))
            {
                writer.Line("return Parse(s, provider);");
            }

            writer.Line();

            using (writer.Block($"public static bool TryParse({System_ReadOnlySpan}<char> s, {System_IFormatProvider}? provider, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                using (writer.Block($"if (InvokeTryParse<{model.PrimitiveTypeName}>(s, provider, out var primitive))"))
                {
                    writer.Line("return TryFrom(primitive, out result);");
                }

                writer.Line();
                writer.Line("result = null;");
                writer.Line("return false;");
                writer.Line();

                using (writer.Block($"static bool InvokeTryParse<T>({System_ReadOnlySpan}<char> s, {System_IFormatProvider}? provider, out T result) where T : {System_ISpanParsable}<T>"))
                {
                    writer.Line("return T.TryParse(s, provider, out result!);");
                }
            }

            writer.Line();

            using (writer.Block($"static bool {System_ISpanParsable}<{model.TypeName}>.TryParse({System_ReadOnlySpan}<char> s, {System_IFormatProvider}? provider, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                writer.Line("return TryParse(s, provider, out result);");
            }
        }
    }

    private static void EmitStringSpecialCase(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_ISpanParsable}<{model.TypeName}>"))
        {
            using (writer.Block($"static {model.TypeName} {System_ISpanParsable}<{model.TypeName}>.Parse({System_ReadOnlySpan}<char> s, {System_IFormatProvider}? provider)"))
            {
                writer.Line("return From(s.ToString());");
            }

            writer.Line();

            using (writer.Block($"static bool {System_ISpanParsable}<{model.TypeName}>.TryParse({System_ReadOnlySpan}<char> s, {System_IFormatProvider}? provider, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                writer.Line("return TryFrom(s.ToString(), out result);");
            }
        }
    }
}
