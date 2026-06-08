using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class ParsableFeatureEmitter : LiftedFeatureEmitter
{
    public override string TargetInterfaceMetadataName => System_IParsable.MetadataName(arity: 1);

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        if (model.PrimitiveTypeName == "string")
        {
            EmitStringSpecialCase(writer, model);
            return;
        }

        using (writer.Block($"partial class {model.TypeName} : {System_IParsable}<{model.TypeName}>"))
        {
            using (writer.Block($"public static {model.TypeName} Parse(string s, {System_IFormatProvider}? provider)"))
            {
                writer.Line($"return Create(InvokeParse<{model.PrimitiveTypeName}>(s, provider));");
                writer.Line();

                using (writer.Block($"static T InvokeParse<T>(string s, {System_IFormatProvider}? provider) where T : {System_IParsable}<T>"))
                {
                    writer.Line("return T.Parse(s, provider);");
                }
            }

            writer.Line();

            using (writer.Block($"public static bool TryParse([{System_Diagnostics_CodeAnalysis_NotNullWhenAttribute}(true)] string? s, {System_IFormatProvider}? provider, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                using (writer.Block($"if (InvokeTryParse<{model.PrimitiveTypeName}>(s, provider, out var primitive))"))
                {
                    writer.Line("return TryCreate(primitive, out result);");
                }

                writer.Line();
                writer.Line("result = null;");
                writer.Line("return false;");
                writer.Line();

                using (writer.Block($"static bool InvokeTryParse<T>(string? s, {System_IFormatProvider}? provider, out T result) where T : {System_IParsable}<T>"))
                {
                    writer.Line("return T.TryParse(s, provider, out result!);");
                }
            }
        }
    }

    private static void EmitStringSpecialCase(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_IParsable}<{model.TypeName}>"))
        {
            using (writer.Block($"static {model.TypeName} {System_IParsable}<{model.TypeName}>.Parse(string s, {System_IFormatProvider}? provider)"))
            {
                writer.Line("return Create(s);");
            }

            writer.Line();

            using (writer.Block($"static bool {System_IParsable}<{model.TypeName}>.TryParse(string? s, {System_IFormatProvider}? provider, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                using (writer.Block("if (s is null)"))
                {
                    writer.Line("result = null;");
                    writer.Line("return false;");
                }

                writer.Line();
                writer.Line("return TryCreate(s, out result);");
            }
        }
    }
}
