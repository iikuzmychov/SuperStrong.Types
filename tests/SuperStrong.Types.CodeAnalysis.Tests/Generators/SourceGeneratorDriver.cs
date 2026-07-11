using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SuperStrong.Types.CodeAnalysis.Generators;

namespace SuperStrong.Types.CodeAnalysis.Tests.Generators;

internal static class SourceGeneratorDriver
{
    public static bool IsPostInitializationSource(GeneratedSourceResult source)
    {
        return IsStrongTypeAttributesSource(source) || IsEmbeddedAttributeSource(source);
    }

    public static bool IsEmbeddedAttributeSource(GeneratedSourceResult source)
    {
        return source.HintName is "Microsoft.CodeAnalysis.EmbeddedAttribute.cs";
    }

    public static bool IsStrongTypeAttributesSource(GeneratedSourceResult source)
    {
        return source.HintName is EmbeddedSources.StrongTypeAttributesHintName;
    }

    public static GeneratorDriver Run(IIncrementalGenerator generator, string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            source,
            new CSharpParseOptions(TestWorkspace.CSharpVersion));

        var compilation = CSharpCompilation.Create(
            assemblyName: "GeneratorTests",
            syntaxTrees: [syntaxTree],
            references: TestWorkspace.References,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(generator);

        return driver.RunGenerators(compilation);
    }
}
