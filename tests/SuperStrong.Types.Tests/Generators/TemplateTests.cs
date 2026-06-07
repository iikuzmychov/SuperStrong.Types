namespace SuperStrong.Types.Tests.Generators;

public sealed class TemplateTests
{
    [Fact]
    public Task Strong_type_with_template_uses_template_definition()
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Snippets.DisableAllFeatures()}}

            namespace Sample;

            public sealed class TestTemplate : IStrongTypeTemplate<int>
            {
                public static StrongTypeDefinition<int> Definition => StrongType.Define<int>();
            }

            [StrongType<int, TestTemplate>]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }

    [Fact]
    public Task Template_carries_feature_attribute_to_strong_type()
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Snippets.DisableAllFeatures()}}

            namespace Sample;

            [StrongTypeFeatures.Lifting.Parsable(IsEnabled = true)]
            public sealed class TestTemplate : IStrongTypeTemplate<int>
            {
                public static StrongTypeDefinition<int> Definition => StrongType.Define<int>();
            }

            [StrongType<int, TestTemplate>]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }

    [Fact]
    public Task Type_level_attribute_overrides_template_attribute()
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Snippets.DisableAllFeatures()}}

            namespace Sample;

            [StrongTypeFeatures.Lifting.Parsable(IsEnabled = false)]
            public sealed class TestTemplate : IStrongTypeTemplate<int>
            {
                public static StrongTypeDefinition<int> Definition => StrongType.Define<int>();
            }

            [StrongType<int, TestTemplate>]
            [StrongTypeFeatures.Lifting.Parsable(IsEnabled = true)]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }
}
