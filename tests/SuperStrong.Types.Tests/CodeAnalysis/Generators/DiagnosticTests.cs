using SuperStrong.Types.CodeAnalysis.Generators;

namespace SuperStrong.Types.Tests.CodeAnalysis.Generators;

public sealed class DiagnosticTests
{
    [Fact]
    public Task Generates_SST001_when_StrongType_attributes_conflict()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            public sealed class TestTemplate : IStrongTypeTemplate<int>
            {
                public static StrongTypeDefinition<int> Define() => StrongType.Define<int>();
            }

            [StrongType<int>]
            [StrongType<int, TestTemplate>]
            public sealed partial class TestStrongType;
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }

    [Fact]
    public Task Generates_SST002_when_strong_type_is_not_partial()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed class TestStrongType;
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }

    [Fact]
    public Task Generates_SST003_when_strong_type_is_a_record()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial record TestStrongType;
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }

    [Fact]
    public Task Generates_SST004_when_strong_type_has_base_type()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            public abstract class TestBaseClass;

            [StrongType<int>]
            public sealed partial class TestStrongType : TestBaseClass;
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }

    [Fact]
    public Task Generates_SST006_when_strong_type_is_abstract()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public abstract partial class TestStrongType;
            """;

        var driver = SourceGeneratorDriver.Run(new StrongTypeGenerator(), source);

        return Verify(driver);
    }
}
