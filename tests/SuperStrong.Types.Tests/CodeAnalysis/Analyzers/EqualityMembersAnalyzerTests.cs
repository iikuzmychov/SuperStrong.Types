using SuperStrong.Types.CodeAnalysis.Analyzers;

namespace SuperStrong.Types.Tests.CodeAnalysis.Analyzers;

public sealed class EqualityMembersAnalyzerTests
{
    [Fact]
    public async Task Reports_warning_when_only_GetHashCode_is_overridden()
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

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new EqualityMembersAnalyzer(), source);

        await Verify(diagnostics);
    }

    [Fact]
    public async Task Reports_warning_when_only_Equals_is_overridden()
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

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new EqualityMembersAnalyzer(), source);

        await Verify(diagnostics);
    }

    [Fact]
    public async Task Reports_warning_for_nested_strong_type()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            public partial class Container
            {
                [StrongType<int>]
                public sealed partial class Inner
                {
                    public override int GetHashCode() => _value.GetHashCode();
                }
            }
            """;

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new EqualityMembersAnalyzer(), source);

        await Verify(diagnostics);
    }

    [Fact]
    public async Task Does_not_report_warning_when_Equals_and_GetHashCode_are_both_overridden()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public bool Equals(TestStrongType? other) => other is not null && _value.Equals(other._value);

                public override int GetHashCode() => _value.GetHashCode();
            }
            """;

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new EqualityMembersAnalyzer(), source);

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task Does_not_report_warning_when_neither_Equals_nor_GetHashCode_is_overridden()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType;
            """;

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new EqualityMembersAnalyzer(), source);

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task Does_not_report_warning_for_non_strong_type_class()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            public sealed class NotAStrongType
            {
                public override int GetHashCode() => 0;
            }
            """;

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new EqualityMembersAnalyzer(), source);

        Assert.Empty(diagnostics);
    }
}
