using System.Diagnostics.CodeAnalysis;

namespace SuperStrong.Types;

public interface IStrongType<TSelf, TPrimitive> : IHasStrongTypeDefinition<TPrimitive>
    where TPrimitive : notnull
{
    public static abstract TSelf Create(TPrimitive value);

    public static abstract bool TryCreate(TPrimitive value, [MaybeNullWhen(false)] out TSelf result);

    public TPrimitive AsPrimitive();
}
