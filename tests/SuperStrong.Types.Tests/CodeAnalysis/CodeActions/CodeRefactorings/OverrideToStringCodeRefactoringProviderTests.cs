using SuperStrong.Types.CodeAnalysis.CodeActions;

namespace SuperStrong.Types.Tests.CodeAnalysis.CodeActions.CodeRefactorings;

public sealed class OverrideToStringCodeRefactoringProviderTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Inserts_ToString_override_when_strong_type_does_not_declare_it(bool hasBlockBody)
    {
        var typeBody = hasBlockBody ? "\n{\n}" : ";";

        var source = $$"""
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType{{typeBody}}
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideToStringCodeRefactoringProvider(), source);

        await Verify(result).UseParameters(hasBlockBody);
    }

    [Fact]
    public async Task Inserts_ToString_override_for_template_based_strong_type()
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

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideToStringCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Does_not_offer_refactoring_when_ToString_is_already_overridden()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public override string ToString() => "custom";
            }
            """;

        var isRefactoringOffered = await CodeRefactoringDriver.OffersRefactoringAsync(new OverrideToStringCodeRefactoringProvider(), source);

        Assert.False(isRefactoringOffered);
    }

    [Fact]
    public async Task Inserts_ToString_override_after_existing_members()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public void ExistingMethod()
                {
                }
            }
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideToStringCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_ToString_override_for_nested_strong_type()
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

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideToStringCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Does_not_offer_refactoring_for_non_strong_type_class()
    {
        var source = """
            namespace Sample;

            public sealed partial class NotAStrongType;
            """;

        var isRefactoringOffered = await CodeRefactoringDriver.OffersRefactoringAsync(new OverrideToStringCodeRefactoringProvider(), source);

        Assert.False(isRefactoringOffered);
    }
}
