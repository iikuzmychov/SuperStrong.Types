using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace SuperStrong.Types.HotChocolate.Directives;

public sealed class PrimitiveDeclaration<TScalar>
    where TScalar : ScalarType
{
    private PrimitiveDeclaration()
    {
    }

    public static PrimitiveDeclaration<TScalar> Instance { get; } = new();

    public PrimitiveDirective Resolve(INamingConventions naming)
    {
        ArgumentNullException.ThrowIfNull(naming);

        return PrimitiveDirective.From(naming.GetTypeName(typeof(TScalar)));
    }
}
