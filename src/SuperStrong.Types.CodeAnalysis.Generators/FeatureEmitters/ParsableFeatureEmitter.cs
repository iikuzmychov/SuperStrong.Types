using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

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
            using (writer.MemberBlock($"public static {model.TypeName} Parse(string s, {System_IFormatProvider}? provider)"))
            {
                writer.Line($"return From(InvokeParse<{model.PrimitiveTypeName}>(s, provider));");
                writer.Line();

                using (writer.Block($"static T InvokeParse<T>(string s, {System_IFormatProvider}? provider) where T : {System_IParsable}<T>"))
                {
                    writer.Line("return T.Parse(s, provider);");
                }
            }

            writer.Line();

            using (writer.MemberBlock($"static {model.TypeName} {System_IParsable}<{model.TypeName}>.Parse(string s, {System_IFormatProvider}? provider)"))
            {
                writer.Line("return Parse(s, provider);");
            }

            writer.Line();

            using (writer.MemberBlock($"public static bool TryParse([{System_Diagnostics_CodeAnalysis_NotNullWhenAttribute}(true)] string? s, {System_IFormatProvider}? provider, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                using (writer.Block($"if (InvokeTryParse<{model.PrimitiveTypeName}>(s, provider, out var primitive))"))
                {
                    writer.Line("return TryFrom(primitive, out result);");
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

            writer.Line();

            using (writer.MemberBlock($"static bool {System_IParsable}<{model.TypeName}>.TryParse(string? s, {System_IFormatProvider}? provider, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                writer.Line("return TryParse(s, provider, out result);");
            }
        }
    }

    private static void EmitStringSpecialCase(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_IParsable}<{model.TypeName}>"))
        {
            // The explicit IParsable member qualifier is emitted without a global:: prefix on purpose.
            // ASP.NET Core's minimal-API binder finds the explicit TryParse by reconstructing the metadata
            // name as "System.IParsable<...>.TryParse"; Roslyn bakes the source qualifier into that name, so a
            // global:: prefix makes the lookup miss and route/query binding throws for string-backed strong
            // types. See https://github.com/dotnet/aspnetcore/issues/58136. Non-string types are unaffected
            // because they also emit a public TryParse that the binder finds by signature.
            writer.MemberLine($"static {model.TypeName} {System_IParsable.FullyQualifiedName}<{model.TypeName}>.Parse(string s, {System_IFormatProvider}? provider) => From(s);");
            writer.Line();
            writer.MemberLine($"static bool {System_IParsable.FullyQualifiedName}<{model.TypeName}>.TryParse(string? s, {System_IFormatProvider}? provider, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result) => TryFrom(s, out result);");
        }
    }
}
