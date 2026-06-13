using SuperStrong.Types.CodeAnalysis.Analyzers;
using SuperStrong.Types.CodeAnalysis.CodeActions;

namespace SuperStrong.Types.Tests.CodeAnalysis.CodeActions.CodeFixes;

public sealed class EqualityMembersCodeFixProviderTests
{
    [Fact]
    public async Task Adds_Equals_when_only_GetHashCode_is_overridden()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public override int GetHashCode() => _value.GetHashCode();
            }
            """;

        var result = await CodeFixDriver.ApplyAsync(new EqualityMembersAnalyzer(), new EqualityMembersCodeFixProvider(), source);

        await Verify(result);
    }

    [Fact]
    public async Task Adds_GetHashCode_when_only_Equals_is_overridden()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public bool Equals(TestStrongType? other) => other is not null && _value.Equals(other._value);
            }
            """;

        var result = await CodeFixDriver.ApplyAsync(new EqualityMembersAnalyzer(), new EqualityMembersCodeFixProvider(), source);

        await Verify(result);
    }
}
