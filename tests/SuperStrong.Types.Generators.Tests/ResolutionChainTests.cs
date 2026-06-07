namespace SuperStrong.Types.Generators.Tests;

public sealed class ResolutionChainTests
{
    [Fact]
    public Task Built_in_default_applies_when_no_attribute_declared()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }

    [Fact]
    public Task Assembly_attribute_wins_over_builtin_default()
    {
        var source = """
            using SuperStrong.Types;

            [assembly: StrongTypeFeatures.Lifting.Parsable(IsEnabled = false)]

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }

    [Fact]
    public Task Template_attribute_wins_over_builtin_default()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongTypeFeatures.Lifting.Parsable(IsEnabled = false)]
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
    public Task Template_attribute_wins_over_assembly_attribute()
    {
        var source = """
            using SuperStrong.Types;

            [assembly: StrongTypeFeatures.Lifting.Parsable(IsEnabled = true)]

            namespace Sample;

            [StrongTypeFeatures.Lifting.Parsable(IsEnabled = false)]
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
    public Task Type_attribute_wins_over_assembly_attribute()
    {
        var source = """
            using SuperStrong.Types;

            [assembly: StrongTypeFeatures.Lifting.Parsable(IsEnabled = false)]

            namespace Sample;

            [StrongType<int>]
            [StrongTypeFeatures.Lifting.Parsable(IsEnabled = true)]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }

    [Fact]
    public Task Type_attribute_wins_over_template_attribute()
    {
        var source = """
            using SuperStrong.Types;

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
