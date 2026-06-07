using System.Collections.Immutable;

namespace SuperStrong.Types.Generators.Tests;

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
    ]);

    public static readonly ImmutableArray<string> LiftingFeatures = AllFeatures
        .Where(feature => feature.StartsWith("Lifting."))
        .ToImmutableArray();

    public static string AllFeaturesDisabled()
    {
        return string.Join(
            Environment.NewLine,
            AllFeatures.Select(feature => $"[assembly: StrongTypeFeatures.{feature}(IsEnabled = false)]"));
    }
}
