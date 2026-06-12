namespace SuperStrong.Types.CodeAnalysis.Generators.Models;

internal sealed record LiftedFeatureState(string FeatureName, bool PrimitiveSupports)
    : OptionalFeatureState(FeatureName);
