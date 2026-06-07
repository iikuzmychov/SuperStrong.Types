using System.Collections.Immutable;

namespace SuperStrong.Types.Tests.Generators;

internal static class Constants
{
    public static readonly ImmutableArray<string> AllFeatures = ImmutableArray.Create(
    [
        "Equality.PartialDefinition",
        "Lifting.Parsable",
        "Lifting.SpanParsable",
        "Lifting.Utf8SpanParsable",
        "Lifting.Formattable",
        "Lifting.SpanFormattable",
        "Lifting.Utf8SpanFormattable",
        "Lifting.Comparable",
        "Lifting.Convertible",
    ]);

    public static readonly ImmutableArray<string> LiftingFeatures = AllFeatures
        .Where(feature => feature.StartsWith("Lifting."))
        .ToImmutableArray();
}
