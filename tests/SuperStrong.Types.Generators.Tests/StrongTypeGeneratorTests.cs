namespace SuperStrong.Types.Generators.Tests;

public sealed class StrongTypeGeneratorTests
{
    [Fact]
    public Task Emits_strong_type_for_simple_int_primitive()
    {
        const string source =
            """
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<int>]
            public sealed partial class UserId;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver);
    }
}
