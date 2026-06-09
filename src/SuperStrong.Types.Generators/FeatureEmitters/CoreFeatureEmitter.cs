using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class CoreFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => true;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        writer.Line($"[{System_Diagnostics_DebuggerDisplayAttribute}(\"{{_value}}\")]");

        using (writer.Block($"partial class {model.TypeName} : {SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>"))
        {
            writer.Line($"private readonly {model.PrimitiveTypeName} _value;");
            writer.Line();

            using (writer.Block($"private {model.TypeName}({model.PrimitiveTypeName} value)"))
            {
                writer.Line("_value = value;");
            }

            writer.Line();

            using (writer.Block($"public static {model.TypeName} From({model.PrimitiveTypeName} value)"))
            {
                writer.Line($"{SuperStrong_Types_StrongType}.EnsureValid(value, Definition);");
                writer.Line();
                writer.Line($"return new {model.TypeName}(value);");
            }

            writer.Line();

            using (writer.Block($"static {model.TypeName} {SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>.From({model.PrimitiveTypeName} value)"))
            {
                writer.Line("return From(value);");
            }

            writer.Line();

            using (writer.Block($"public static bool TryFrom({model.PrimitiveTypeName} value, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
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

            using (writer.Block($"static bool {SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>.TryFrom({model.PrimitiveTypeName} value, [{System_Diagnostics_CodeAnalysis_MaybeNullWhenAttribute}(false)] out {model.TypeName} result)"))
            {
                writer.Line("return TryFrom(value, out result);");
            }

            writer.Line();

            writer.Line($"public {model.PrimitiveTypeName} AsPrimitive() => _value;");
            writer.Line();
            using (writer.Block($"{model.PrimitiveTypeName} {SuperStrong_Types_IStrongType}<{model.TypeName}, {model.PrimitiveTypeName}>.AsPrimitive()"))
            {
                writer.Line("return AsPrimitive();");
            }
        }
    }
}
