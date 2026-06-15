namespace SuperStrong.Types.HotChocolate.Directives;

public sealed record RegexDirective
{
    public required string Pattern { get; init; }

    public string? Options { get; init; }
}
