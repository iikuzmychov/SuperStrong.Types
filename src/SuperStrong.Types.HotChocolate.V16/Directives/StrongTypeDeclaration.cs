using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace SuperStrong.Types.HotChocolate.Directives;

public sealed class StrongTypeDeclaration<TScalar>
    where TScalar : ScalarType
{
    private StrongTypeDeclaration()
    {
    }

    public static StrongTypeDeclaration<TScalar> Instance { get; } = new();

    public StrongTypeDirective Resolve(INamingConventions naming)
    {
        ArgumentNullException.ThrowIfNull(naming);

        return StrongTypeDirective.From(naming.GetTypeName(typeof(TScalar)));
    }
}
