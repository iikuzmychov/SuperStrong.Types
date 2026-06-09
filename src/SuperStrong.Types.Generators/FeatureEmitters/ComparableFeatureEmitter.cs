using SuperStrong.Types.Generators.Helpers;
using SuperStrong.Types.Generators.Models;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal sealed class ComparableFeatureEmitter : LiftedFeatureEmitter
{
    public override string TargetInterfaceMetadataName => System_IComparable.MetadataName(arity: 1);

    public override void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        using (writer.Block($"partial class {model.TypeName} : {System_IComparable}<{model.TypeName}>"))
        {
            using (writer.MemberBlock($"public int CompareTo({model.TypeName}? other)"))
            {
                using (writer.Block("if (other is null)"))
                {
                    writer.Line("return 1;");
                }
                writer.Line();
                writer.Line($"return InvokeCompareTo<{model.PrimitiveTypeName}>(_value, other._value);");
                writer.Line();

                using (writer.Block($"static int InvokeCompareTo<T>(T value, T other) where T : {System_IComparable}<T>"))
                {
                    writer.Line("return value.CompareTo(other);");
                }
            }

            writer.Line();

            using (writer.MemberBlock($"int {System_IComparable}<{model.TypeName}>.CompareTo({model.TypeName}? other)"))
            {
                writer.Line("return CompareTo(other);");
            }
        }
    }
}
