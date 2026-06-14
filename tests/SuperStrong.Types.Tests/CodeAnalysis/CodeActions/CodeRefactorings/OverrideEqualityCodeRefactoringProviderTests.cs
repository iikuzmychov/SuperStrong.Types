using SuperStrong.Types.CodeAnalysis.CodeActions;

namespace SuperStrong.Types.Tests.CodeAnalysis.CodeActions.CodeRefactorings;

public sealed class OverrideEqualityCodeRefactoringProviderTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Inserts_Equals_and_GetHashCode_when_neither_is_declared(bool hasBlockBody)
    {
        var typeBody = hasBlockBody ? "\n{\n}" : ";";

        var source = $$"""
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType{{typeBody}}
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideEqualityCodeRefactoringProvider(), source);

        await Verify(result).UseParameters(hasBlockBody);
    }

    [Fact]
    public async Task Inserts_Equals_and_GetHashCode_after_existing_members()
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

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideEqualityCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_Equals_and_GetHashCode_for_nested_strong_type()
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

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideEqualityCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_only_Equals_when_GetHashCode_is_already_declared()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public override int GetHashCode() => 0;
            }
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideEqualityCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_Equals_and_GetHashCode_for_template_based_strong_type()
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

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideEqualityCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Inserts_only_GetHashCode_when_Equals_is_already_declared()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public bool Equals(TestStrongType? other) => other is not null;
            }
            """;

        var result = await CodeRefactoringDriver.ApplyAsync(new OverrideEqualityCodeRefactoringProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Does_not_offer_refactoring_when_Equals_and_GetHashCode_are_already_declared()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public bool Equals(TestStrongType? other) => other is not null;
                public override int GetHashCode() => 0;
            }
            """;

        var isRefactoringOffered = await CodeRefactoringDriver.OffersRefactoringAsync(new OverrideEqualityCodeRefactoringProvider(), source);

        Assert.False(isRefactoringOffered);
    }
}
