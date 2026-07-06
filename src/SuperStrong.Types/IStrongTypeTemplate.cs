namespace SuperStrong.Types;

public interface IStrongTypeTemplate<TPrimitive>
    where TPrimitive : notnull
{
    public static abstract StrongTypeDefinition<TPrimitive> Define();
}
