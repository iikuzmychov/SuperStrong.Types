namespace SuperStrong.Types;

public interface IHasStrongTypeLayout<TPrimitive>
    where TPrimitive : notnull
{
    public static abstract StrongTypeLayout<TPrimitive> Layout { get; }
}
