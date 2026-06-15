using System.Collections.Immutable;

namespace SuperStrong.Types.HotChocolate.Directives;

public sealed record AllowedValuesDirective
{
    public required ImmutableArray<object> Values { get; init; }
}
