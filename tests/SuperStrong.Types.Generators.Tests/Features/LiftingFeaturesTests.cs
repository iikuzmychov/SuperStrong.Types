namespace SuperStrong.Types.Generators.Tests.Features;

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

            {{Constants.AllFeaturesDisabled()}}

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

            {{Constants.AllFeaturesDisabled()}}

            namespace Sample;

            [StrongType<int>]
            [StrongTypeFeatures.{{feature}}(IsEnabled = false)]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver).UseParameters(feature);
    }

    [Fact]
    public Task Skips_feature_when_user_declared_target_interface()
    {
        var source = $$"""
            using System;
            using SuperStrong.Types;

            {{Constants.AllFeaturesDisabled()}}

            namespace Sample;

            [StrongType<int>]
            [StrongTypeFeatures.Lifting.Parsable(IsEnabled = true)]
            public sealed partial class TestStrongType : IParsable<TestStrongType>;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }

    [Fact]
    public Task Skips_feature_when_primitive_lacks_support()
    {
        // string doesn't implement IFormattable, ISpanFormattable, IUtf8SpanFormattable, IUtf8SpanParsable
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<string>]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);
        
        return Verify(driver);
    }
}
