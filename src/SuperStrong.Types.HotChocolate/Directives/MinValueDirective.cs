namespace SuperStrong.Types.HotChocolate.Directives;

public sealed record MinValueDirective
{
    public required object Value { get; init; }

    public required bool IsExclusive { get; init; }
}
