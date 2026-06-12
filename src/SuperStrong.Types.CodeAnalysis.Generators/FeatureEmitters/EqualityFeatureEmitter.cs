using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using SuperStrong.Types.CodeAnalysis.Generators.Models;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal sealed class EqualityFeatureEmitter : IStrongTypeFeatureEmitter
{
    public bool ShouldEmit(StrongTypeModel model) => true;

    public void Emit(IndentedWriter writer, StrongTypeModel model)
    {
        var equalityOperatorsInterface = $"{System_Numerics_IEqualityOperators}<{model.TypeName}, {model.TypeName}, bool>";

        using (writer.Block($"partial class {model.TypeName} : {System_IEquatable}<{model.TypeName}>, {equalityOperatorsInterface}"))
        {
            if (!model.UserOverridesEquals)
            {
                writer.MemberLine($"public bool Equals({model.TypeName}? other) => other is not null && _value.Equals(other._value);");
                writer.Line();
            }

            using (writer.MemberBlock($"bool {System_IEquatable}<{model.TypeName}>.Equals({model.TypeName}? other)"))
            {
                writer.Line("return Equals(other);");
            }

            writer.Line();

            using (writer.MemberBlock($"public static bool operator ==({model.TypeName}? left, {model.TypeName}? right)"))
            {
                using (writer.Block("if (left is null)"))
                {
                    writer.Line("return right is null;");
                }

                writer.Line();
                writer.Line("return left.Equals(right);");
            }

            writer.Line();

            using (writer.MemberBlock($"static bool {equalityOperatorsInterface}.operator ==({model.TypeName}? left, {model.TypeName}? right)"))
            {
                writer.Line("return left == right;");
            }

            writer.Line();
            writer.MemberLine($"public static bool operator !=({model.TypeName}? left, {model.TypeName}? right) => !(left == right);");
            writer.Line();

            using (writer.MemberBlock($"static bool {equalityOperatorsInterface}.operator !=({model.TypeName}? left, {model.TypeName}? right)"))
            {
                writer.Line("return left != right;");
            }

            writer.Line();
            writer.MemberLine($"public override bool Equals(object? obj) => obj is {model.TypeName} other && Equals(other);");

            if (!model.UserOverridesGetHashCode)
            {
                writer.Line();
                writer.MemberLine("public override int GetHashCode() => _value.GetHashCode();");
            }
        }
    }
}
