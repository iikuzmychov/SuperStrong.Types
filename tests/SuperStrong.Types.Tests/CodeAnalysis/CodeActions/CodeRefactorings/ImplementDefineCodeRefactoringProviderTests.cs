using SuperStrong.Types.CodeAnalysis.CodeActions;

namespace SuperStrong.Types.Tests.CodeAnalysis.CodeActions.CodeRefactorings;

public sealed class ImplementDefineCodeRefactoringProviderTests
{
    [Fact]
    public async Task Offers_partial_and_explicit_implement_actions()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType;
            """;

        var actions = await CodeRefactoringDriver.GetActionsAsync(new ImplementDefineCodeRefactoringProvider(), source);

        Assert.Equal(["Implement Define()", "Implement Define() explicitly"], actions.Select(action => action.Title));
    }

    [Theory]
    [InlineData("int", false)]
    [InlineData("int", true)]
    [InlineData("string", false)]
    public async Task Inserts_Define_method_when_strong_type_does_not_declare_it(string primitive, bool hasBlockBody)
    {
        var typeBody = hasBlockBody ? "\n{\n}" : ";";

        var source = $$"""
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<{{primitive}}>]
            public sealed partial class TestStrongType{{typeBody}}
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new ImplementDefineCodeRefactoringProvider(), source, "Implement Define()");

        await Verify(result).UseParameters(primitive, hasBlockBody);
    }

    [Fact]
    public async Task Inserts_explicit_Define_implementation()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType;
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new ImplementDefineCodeRefactoringProvider(), source, "Implement Define() explicitly");

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_Define_method_after_public_static_methods()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public static string Describe() => "Test";

                public void Refresh()
                {
                }
            }
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new ImplementDefineCodeRefactoringProvider(), source, "Implement Define()");

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_Define_method_after_instance_methods()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public string Name { get; } = "";

                public void Refresh()
                {
                }
            }
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new ImplementDefineCodeRefactoringProvider(), source, "Implement Define()");

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_Define_method_after_properties_when_there_are_no_methods()
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

        var result = await CodeRefactoringDriver.ApplyAsync(new ImplementDefineCodeRefactoringProvider(), source, "Implement Define()");

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_Define_method_for_nested_strong_type()
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

        var result = await CodeRefactoringDriver.ApplyAsync(new ImplementDefineCodeRefactoringProvider(), source, "Implement Define()");

        await Verify(result);
    }

    [Fact]
    public async Task Does_not_offer_refactoring_when_Define_is_already_declared()
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

        var isRefactoringOffered = await CodeRefactoringDriver.OffersRefactoringAsync(new ImplementDefineCodeRefactoringProvider(), source);

        Assert.False(isRefactoringOffered);
    }

    [Fact]
    public async Task Does_not_offer_refactoring_when_Define_is_declared_partially()
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

        var isRefactoringOffered = await CodeRefactoringDriver.OffersRefactoringAsync(new ImplementDefineCodeRefactoringProvider(), source);

        Assert.False(isRefactoringOffered);
    }

    [Fact]
    public async Task Does_not_offer_refactoring_when_Define_is_implemented_explicitly()
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

        var isRefactoringOffered = await CodeRefactoringDriver.OffersRefactoringAsync(new ImplementDefineCodeRefactoringProvider(), source);

        Assert.False(isRefactoringOffered);
    }
}
