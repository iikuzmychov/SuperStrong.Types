namespace SuperStrong.Types.Tests.Generators;

public sealed class PrimitiveTests
{
    [Theory]
    [InlineData("int")]
    [InlineData("long")]
    [InlineData("decimal")]
    [InlineData("double")]
    [InlineData("float")]
    [InlineData("string")]
    [InlineData("Guid")]
    [InlineData("DateTime")]
    [InlineData("DateTimeOffset")]
    [InlineData("TimeSpan")]
    public Task Generates_strong_type(string primitive)
    {
        var source = $$"""
            using System;
            using SuperStrong.Types;

            namespace Sample;

            [StrongType<{{primitive}}>]
            public sealed partial class TestStrongType;
            """;

        var driver = StrongTypeGeneratorDriver.Run(source);

        return Verify(driver).UseParameters(primitive);
    }
}
