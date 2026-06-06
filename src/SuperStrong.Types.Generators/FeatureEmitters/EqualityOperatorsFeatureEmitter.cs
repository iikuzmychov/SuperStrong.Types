using Microsoft.CodeAnalysis;
using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class EqualityOperatorsFeatureEmitter : LiftedFeatureEmitter
{
    public override string FeatureAttributeMetadataName => SuperStrong_Types_StrongTypeEqualityOperatorsFeatureAttribute.MetadataName();
    public override string TargetInterfaceMetadataName => System_Numerics_IEqualityOperators.MetadataName(arity: 3);
    public override bool IsEnabledByDefault => true;

    protected override ITypeSymbol[] GetTypeArguments(ITypeSymbol primarySymbol, Compilation compilation)
    {
        return [primarySymbol, primarySymbol, compilation.GetSpecialType(SpecialType.System_Boolean)];
    }

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_Numerics_IEqualityOperators}<{model.TypeName}, {model.TypeName}, bool>"))
        {
            using (writer.Block($"public static bool operator ==({model.TypeName}? left, {model.TypeName}? right)"))
            {
                using (writer.Block("if (left is null)"))
                {
                    writer.Line("return right is null;");
                }

                writer.Line();
                writer.Line("return left.Equals(right);");
            }

            writer.Line();
            writer.Line($"public static bool operator !=({model.TypeName}? left, {model.TypeName}? right) => !(left == right);");
        }
    }
}
