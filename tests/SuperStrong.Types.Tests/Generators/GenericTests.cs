namespace SuperStrong.Types.Tests.Generators;

public sealed class GenericTests
{
    [Theory]
    [InlineData("int")]
    [InlineData("long")]
    [InlineData("decimal")]
    [InlineData("double")]
    [InlineData("float")]
    [InlineData("string")]
    [InlineData("Guid")]
    [InlineData("DateTime")]
    [InlineData("DateTimeOffset")]
    [InlineData("TimeSpan")]
    public Task Generates_strong_type(string primitive)
    {
        var source = $$"""
            using System;
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<{{primitive}}>]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver).UseParameters(primitive);
    }

    [Theory]
    [InlineData("class")]
    [InlineData("struct")]
    [InlineData("record")]
    [InlineData("record struct")]
    public Task Generates_correct_keyword_for_nested_strong_type(string ancestorKeyword)
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Snippets.DisableAllFeatures()}}

            namespace Sample;

            public partial {{ancestorKeyword}} Container
            {
                [StrongType<int>]
                public sealed partial class TestStrongType;
            }
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver).UseParameters(ancestorKeyword.Replace(' ', '_'));
    }

    [Fact]
    public Task Generates_definition_when_user_does_not_declare_interface()
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Snippets.DisableAllFeatures()}}

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }

    [Fact]
    public Task Skips_definition_generating_when_user_declares_interface()
    {
        var source = $$"""
            using SuperStrong.Types;

            {{Snippets.DisableAllFeatures()}}

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType : IHasStrongTypeDefinition<int>
            {
                public static StrongTypeDefinition<int> Definition { get; } = StrongType.Define<int>();
            }
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }
}
