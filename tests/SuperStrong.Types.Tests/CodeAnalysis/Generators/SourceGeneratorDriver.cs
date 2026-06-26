using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SuperStrong.Types.Tests.CodeAnalysis.Generators;

internal static class SourceGeneratorDriver
{
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
