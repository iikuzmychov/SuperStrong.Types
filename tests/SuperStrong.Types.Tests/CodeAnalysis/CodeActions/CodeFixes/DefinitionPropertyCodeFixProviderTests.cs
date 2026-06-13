using SuperStrong.Types.CodeAnalysis.Analyzers;
using SuperStrong.Types.CodeAnalysis.CodeActions;

namespace SuperStrong.Types.Tests.CodeAnalysis.CodeActions.CodeFixes;

public sealed class DefinitionPropertyCodeFixProviderTests
{
    [Fact]
    public async Task Converts_expression_bodied_Definition_property_to_get_only_property()
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

        var result = await CodeFixDriver.ApplyAsync(new DefinitionPropertyAnalyzer(), new DefinitionPropertyCodeFixProvider(), source);

        await Verify(result);
    }
}
