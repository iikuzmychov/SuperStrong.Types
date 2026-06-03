namespace SuperStrong.Types;

public interface IStrongType<TSelf, TPrimitive> : IHasStrongTypeDefinition<TPrimitive>, IHasStrongTypeLayout<TPrimitive>
    where TPrimitive : notnull
{
    public static abstract TSelf Create(TPrimitive value);

    public TPrimitive AsPrimitive();
}
