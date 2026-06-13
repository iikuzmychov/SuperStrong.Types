using SuperStrong.Types.CodeAnalysis.CodeActions;

namespace SuperStrong.Types.Tests.CodeAnalysis.CodeActions.CodeRefactorings;

public sealed class AddDefinitionCodeRefactoringProviderTests
{
    [Theory]
    [InlineData("int", false)]
    [InlineData("int", true)]
    [InlineData("string", false)]
    public async Task Inserts_Definition_property_when_strong_type_does_not_declare_it(string primitive, bool hasBlockBody)
    {
        var typeBody = hasBlockBody ? "\n{\n}" : ";";

        var source = $$"""
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<{{primitive}}>]
            public sealed partial class TestStrongType{{typeBody}}
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new AddDefinitionCodeRefactoringProvider(), source);

        await Verify(result).UseParameters(primitive, hasBlockBody);
    }

    [Fact]
    public async Task Inserts_Definition_property_after_public_static_properties()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public string Name { get; } = "";

                public static string DisplayName => "Test";

                public string Description { get; } = "";
            }
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new AddDefinitionCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_Definition_property_for_nested_strong_type()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            public partial class Container
            {
                [StrongType<int>]
                public sealed partial class Inner;
            }
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new AddDefinitionCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Does_not_offer_refactoring_when_Definition_is_already_declared()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public static StrongTypeDefinition<int> Definition { get; } = StrongType.Define<int>();
            }
            """;

        var isRefactoringOffered = await CodeRefactoringDriver.OffersRefactoringAsync(new AddDefinitionCodeRefactoringProvider(), source);

        Assert.False(isRefactoringOffered);
    }
}
