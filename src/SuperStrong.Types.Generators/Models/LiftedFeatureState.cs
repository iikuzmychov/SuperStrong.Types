namespace SuperStrong.Types.Generators.Models;

internal sealed record LiftedFeatureState(string FeatureName, bool IsEnabled, bool PrimitiveSupports)
    : OptionalFeatureState(FeatureName, IsEnabled);
