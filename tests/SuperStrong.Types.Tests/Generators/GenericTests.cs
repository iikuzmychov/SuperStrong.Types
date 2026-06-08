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
    public Task Strong_type_with_template_uses_template_definition()
    {
        var source = """
            using SuperStrong.Types;

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
    public Task Skips_definition_generating_when_user_declares_interface()
    {
        var source = """
            using SuperStrong.Types;

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

    [Fact]
    public Task Skips_Equals_generating_when_user_declares_it()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<string>]
            public sealed partial class TestStrongType
            {
                public bool Equals(TestStrongType? other)
                {
                    // custom equality logic
                    throw new NotImplementedException();
                }
            }
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }

    [Fact]
    public Task Skips_GetHashCode_generating_when_user_declares_it()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<string>]
            public sealed partial class TestStrongType
            {
                public override int GetHashCode()
                {
                    // custom hash code logic
                    throw new NotImplementedException();
                }
            }
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }
}
