namespace SuperStrong.Types;

public sealed class StrongTypeLayout<TPrimitive>
    where TPrimitive : notnull
{
    internal static StrongTypeLayout<TPrimitive> Empty { get; } = new();

    private StrongTypeLayout()
    {
    }
}
