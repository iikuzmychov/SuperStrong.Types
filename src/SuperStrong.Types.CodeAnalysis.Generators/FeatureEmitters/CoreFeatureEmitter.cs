using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal sealed class CoreFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => true;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        writer.Line($"[{System_Diagnostics_DebuggerDisplayAttribute}(\"{{_value}}\")]");
        writer.Line($"[{System_ComponentModel_TypeConverterAttribute}(typeof({SuperStrong_Types_Converters_StrongTypeConverter}<{model.TypeName}, {model.PrimitiveTypeName}>))]");
        writer.Line($"[{System_Text_Json_Serialization_JsonConverterAttribute}(typeof({SuperStrong_Types_Converters_JsonStrongTypeConverter}<{model.TypeName}, {model.PrimitiveTypeName}>))]");

        using (writer.Block($"partial class {model.TypeName} : {SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>"))
        {
            writer.MemberLine($"private readonly {model.PrimitiveTypeName} _value;");
            writer.Line();

            using (writer.MemberBlock($"private {model.TypeName}({model.PrimitiveTypeName} value)"))
            {
                writer.Line("_value = value;");
            }

            writer.Line();

            using (writer.MemberBlock($"public static {model.TypeName} From({model.PrimitiveTypeName} value)"))
            {
                writer.Line($"{SuperStrong_Types_StrongType}.EnsureValid(value, Definition);");
                writer.Line();
                writer.Line($"return new {model.TypeName}(value);");
            }

            writer.Line();

            using (writer.MemberBlock($"static {model.TypeName} {SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>.From({model.PrimitiveTypeName} value)"))
            {
                writer.Line("return From(value);");
            }

            writer.Line();

            using (writer.MemberBlock($"public static bool TryFrom(" +
                $"[{System_Diagnostics_CodeAnalysis_AllowNullAttribute}] {model.PrimitiveTypeName} value," +
                $"[{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                using (writer.Block($"if ({SuperStrong_Types_StrongType}.IsValid(value, Definition))"))
                {
                    writer.Line($"result = new {model.TypeName}(value);");
                    writer.Line("return true;");
                }

                writer.Line();
                writer.Line("result = null;");
                writer.Line("return false;");
            }

            writer.Line();

            using (writer.MemberBlock($"static bool {SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>.TryFrom(" +
                $"[{System_Diagnostics_CodeAnalysis_AllowNullAttribute}] {model.PrimitiveTypeName} value," +
                $"[{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                writer.Line("return TryFrom(value, out result);");
            }

            writer.Line();

            writer.MemberLine($"public {model.PrimitiveTypeName} AsPrimitive() => _value;");
            writer.Line();

            using (writer.MemberBlock($"{model.PrimitiveTypeName} {SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>.AsPrimitive()"))
            {
                writer.Line("return AsPrimitive();");
            }
        }
    }
}
