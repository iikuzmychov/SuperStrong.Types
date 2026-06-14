using Microsoft.CodeAnalysis;
using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal sealed class ComparisonOperatorsFeatureEmitter : LiftedFeatureEmitter
{
    public override string TargetInterfaceMetadataName => System_Numerics_IComparisonOperators.MetadataName(arity: 3);

    protected override ITypeSymbol[] GetTypeArguments(ITypeSymbol primarySymbol, Compilation compilation)
    {
        return [primarySymbol, primarySymbol, compilation.GetSpecialType(SpecialType.System_Boolean)];
    }

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        var comparisonOperatorsInterface = $"{System_Numerics_IComparisonOperators}<{model.TypeName}, {model.TypeName}, bool>";

        using (writer.Block($"partial class {model.TypeName} : {comparisonOperatorsInterface}"))
        {
            EmitOperator(writer, model, comparisonOperatorsInterface, "<", "LessThan");
            writer.Line();
            EmitOperator(writer, model, comparisonOperatorsInterface, "<=", "LessThanOrEqual");
            writer.Line();
            EmitOperator(writer, model, comparisonOperatorsInterface, ">", "GreaterThan");
            writer.Line();
            EmitOperator(writer, model, comparisonOperatorsInterface, ">=", "GreaterThanOrEqual");
        }
    }

    private static void EmitOperator(IndentedWriter writer, StrongTypeModel model, string comparisonOperatorsInterface, string @operator, string operatorName)
    {
        using (writer.MemberBlock($"public static bool operator {@operator}({model.TypeName} left, {model.TypeName} right)"))
        {
            writer.Line("global::System.ArgumentNullException.ThrowIfNull(left);");
            writer.Line("global::System.ArgumentNullException.ThrowIfNull(right);");
            writer.Line();
            writer.Line($"return Invoke{operatorName}<{model.PrimitiveTypeName}>(left._value, right._value);");
            writer.Line();

            using (writer.Block($"static bool Invoke{operatorName}<T>(T left, T right) where T : {System_Numerics_IComparisonOperators}<T, T, bool>"))
            {
                writer.Line($"return left {@operator} right;");
            }
        }

        writer.Line();

        writer.MemberLine($"static bool {comparisonOperatorsInterface}.operator {@operator}({model.TypeName} left, {model.TypeName} right) => left {@operator} right;");
    }
}
