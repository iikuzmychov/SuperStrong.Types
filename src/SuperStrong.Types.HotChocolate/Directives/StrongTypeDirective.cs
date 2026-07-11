namespace SuperStrong.Types.HotChocolate.Directives;

public sealed record StrongTypeDirective
{
    public required string PrimitiveType { get; init; }

    internal StrongTypeDirective()
    {
    }

    public static StrongTypeDirective From(string type)
    {
        return new StrongTypeDirective { PrimitiveType = type };
    }
}
