namespace SuperStrong.Types.HotChocolate.Directives;

public sealed record PrimitiveDirective
{
    public required string Type { get; init; }

    internal PrimitiveDirective()
    {
    }

    public static PrimitiveDirective From(string type)
    {
        return new PrimitiveDirective { Type = type };
    }
}
