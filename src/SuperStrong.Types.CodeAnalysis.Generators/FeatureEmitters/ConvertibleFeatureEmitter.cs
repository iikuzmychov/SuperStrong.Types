using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal sealed class ConvertibleFeatureEmitter : LiftedFeatureEmitter
{
    public override string TargetInterfaceMetadataName => System_IConvertible.MetadataName();

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_IConvertible}"))
        {
            EmitForward(writer, System_TypeCode.ToString(), "GetTypeCode", "");
            writer.Line();
            EmitForward(writer, "bool", "ToBoolean", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "byte", "ToByte", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "char", "ToChar", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "global::System.DateTime", "ToDateTime", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "decimal", "ToDecimal", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "double", "ToDouble", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "short", "ToInt16", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "int", "ToInt32", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "long", "ToInt64", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "sbyte", "ToSByte", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "float", "ToSingle", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "string", "ToString", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "object", "ToType", $"{System_Type} conversionType, {System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "ushort", "ToUInt16", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "uint", "ToUInt32", $"{System_IFormatProvider}? provider");
            writer.Line();
            EmitForward(writer, "ulong", "ToUInt64", $"{System_IFormatProvider}? provider");
        }
    }

    private static void EmitForward(IndentedWriter writer, string returnType, string methodName, string parameters)
    {
        var argList = parameters.Length == 0
            ? string.Empty
            : string.Join(", ", parameters.Split(',').Select(p => p.Trim().Split(' ').Last()));

        using (writer.MemberBlock($"{returnType} {System_IConvertible}.{methodName}({parameters})"))
        {
            writer.Line($"return (({System_IConvertible})_value).{methodName}({argList});");
        }
    }
}
