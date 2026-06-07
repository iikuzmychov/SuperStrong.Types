namespace SuperStrong.Types.Tests.Generators.Features;

public sealed class LiftingFeaturesTests
{
    public static IEnumerable<TheoryDataRow<string>> LiftingFeatureFixtures => Constants
        .LiftingFeatures
        .Select(feature => new TheoryDataRow<string>(feature));

    [Theory]
    [MemberData(nameof(LiftingFeatureFixtures))]
    public Task Lifts_feature_when_enabled(string feature)
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Snippets.DisableAllFeatures()}}

            namespace Sample;

            [StrongType<int>]
            [StrongTypeFeatures.{{feature}}(IsEnabled = true)]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver).UseParameters(feature);
    }

    [Theory]
    [MemberData(nameof(LiftingFeatureFixtures))]
    public Task Skips_feature_when_disabled(string feature)
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Snippets.DisableAllFeatures()}}

            namespace Sample;

            [StrongType<int>]
            [StrongTypeFeatures.{{feature}}(IsEnabled = false)]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver).UseParameters(feature);
    }

    [Fact]
    public Task Skips_feature_when_primitive_lacks_support()
    {
        // string does not implement IFormattable, so Formattable feature should be skipped for it, even if it's enabled
        var source = $$"""
            using SuperStrong.Types;

            {{Snippets.DisableAllFeatures()}}

            namespace Sample;

            [StrongType<string>]
            [StrongTypeFeatures.Lifting.Formattable(IsEnabled = true)]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);
        
        return Verify(driver);
    }
}
