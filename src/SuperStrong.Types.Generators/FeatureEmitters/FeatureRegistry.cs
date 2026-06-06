using System.Collections.Immutable;

namespace SuperStrong.Types.Generators.FeatureEmitters;

internal static class FeatureRegistry
{
    public static readonly ImmutableArray<IStrongTypeFeatureEmitter> All = ImmutableArray.Create<IStrongTypeFeatureEmitter>(
    [
        new CoreFeatureEmitter(),
        new HasStrongTypeDefinitionFeatureEmitter(),
        new EqualityFeatureEmitter(),
        new ToStringFeatureEmitter(),
        new ParsableFeatureEmitter(),
        new SpanParsableFeatureEmitter(),
        new Utf8SpanParsableFeatureEmitter(),
        new FormattableFeatureEmitter(),
        new SpanFormattableFeatureEmitter(),
        new Utf8SpanFormattableFeatureEmitter(),
        new ComparableFeatureEmitter(),
        new EqualityOperatorsFeatureEmitter(),
    ]);

    public static readonly ImmutableArray<IOptionalFeatureEmitter> Optional = All
        .OfType<IOptionalFeatureEmitter>()
        .ToImmutableArray();
}
