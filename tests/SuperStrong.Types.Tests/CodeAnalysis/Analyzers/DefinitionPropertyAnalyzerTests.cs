using SuperStrong.Types.CodeAnalysis.Analyzers;

namespace SuperStrong.Types.Tests.CodeAnalysis.Analyzers;

public sealed class DefinitionPropertyAnalyzerTests
{
    [Fact]
    public async Task Reports_warning_for_expression_bodied_Definition()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public static StrongTypeDefinition<int> Definition => StrongType.Define<int>();
            }
            """;

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new DefinitionPropertyAnalyzer(), source);

        await Verify(diagnostics);
    }

    [Fact]
    public async Task Reports_warning_for_expression_bodied_Definition_in_nested_strong_type()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            public partial class Container
            {
                [StrongType<int>]
                public sealed partial class Inner
                {
                    public static StrongTypeDefinition<int> Definition => StrongType.Define<int>();
                }
            }
            """;

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new DefinitionPropertyAnalyzer(), source);

        await Verify(diagnostics);
    }

    [Fact]
    public async Task Does_not_report_warning_for_get_only_Definition()
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

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new DefinitionPropertyAnalyzer(), source);

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
                public static StrongTypeDefinition<int> Definition => StrongType.Define<int>();
            }
            """;

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new DefinitionPropertyAnalyzer(), source);

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task Does_not_report_warning_for_instance_Definition()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public StrongTypeDefinition<int> Definition => StrongType.Define<int>();
            }
            """;

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new DefinitionPropertyAnalyzer(), source);

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task Does_not_report_warning_when_Definition_is_not_a_StrongTypeDefinition()
    {
        var source = """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class TestStrongType
            {
                public static int Definition => 0;
            }
            """;

        var diagnostics = await AnalyzerDriver.GetDiagnosticsAsync(new DefinitionPropertyAnalyzer(), source);

        Assert.Empty(diagnostics);
    }
}
