using System.Diagnostics.CodeAnalysis;

namespace SuperStrong.Types;

public interface IStrongType<TSelf, TPrimitive>
    where TPrimitive : notnull
{
    public static abstract StrongTypeDefinition<TPrimitive> Definition { get; }

    public static abstract TSelf From(TPrimitive value);

    public static abstract bool TryFrom(TPrimitive value, [MaybeNullWhen(false)] out TSelf result);

    public TPrimitive AsPrimitive();
}
