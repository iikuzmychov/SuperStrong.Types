namespace SuperStrong.Types;

public interface IHasStrongTypeDefinition<TPrimitive>
    where TPrimitive : notnull
{
    public static abstract StrongTypeDefinition<TPrimitive> Definition { get; }
}