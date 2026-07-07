using SuperStrong.Types.CodeAnalysis.Generators;

namespace SuperStrong.Types.Tests.CodeAnalysis.Generators;

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

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

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

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

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
                public static StrongTypeDefinition<int> Define() => StrongType.Define<int>();
            }

            [StrongType<int, TestTemplate>]
            public sealed partial class TestStrongType;
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }

    [Fact]
    public Task Uses_explicitly_implemented_Define()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                static StrongTypeDefinition<int> IStrongType<TestStrongType, int>.Define() => StrongType.Define<int>();
            }
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }

    [Fact]
    public Task Emits_defining_declaration_for_partial_Define()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public static partial StrongTypeDefinition<int> Define() => StrongType.Define<int>();
            }
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }

    [Fact]
    public Task Emits_defining_declaration_for_non_partial_Define_so_it_does_not_compile()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public static StrongTypeDefinition<int> Define() => StrongType.Define<int>();
            }
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }

    [Fact]
    public Task Emits_defining_declaration_for_partial_Equals()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<string>]
            public sealed partial class TestStrongType
            {
                public partial bool Equals(TestStrongType? other)
                {
                    // custom equality logic
                    throw new NotImplementedException();
                }
            }
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }

    [Fact]
    public Task Emits_defining_declaration_for_non_partial_Equals_so_it_does_not_compile()
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

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

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

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }
}
