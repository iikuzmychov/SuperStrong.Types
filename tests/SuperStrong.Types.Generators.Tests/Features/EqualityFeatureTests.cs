namespace SuperStrong.Types.Generators.Tests.Features;

public sealed class EqualityFeatureTests
{
    [Fact]
    public Task Generates_partial_declarations_when_PartialDefinition_enabled()
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Constants.AllFeaturesDisabled()}}

            namespace Sample;

            [StrongType<int>]
            [StrongTypeFeatures.Equality.PartialDefinition(IsEnabled = true)]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }

    [Fact]
    public Task Generates_full_bodies_when_PartialDefinition_disabled()
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Constants.AllFeaturesDisabled()}}

            namespace Sample;

            [StrongType<int>]
            [StrongTypeFeatures.Equality.PartialDefinition(IsEnabled = false)]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }
}
