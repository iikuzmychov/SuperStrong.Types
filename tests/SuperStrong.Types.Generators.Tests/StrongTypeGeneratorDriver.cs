using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SuperStrong.Types.Generators.Tests;

internal static class StrongTypeGeneratorDriver
{
    private static readonly IReadOnlyList<MetadataReference> References = BuildReferences();

    public static GeneratorDriver Run(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            source,
            new CSharpParseOptions(LanguageVersion.Preview));

        var compilation = CSharpCompilation.Create(
            assemblyName: "GeneratorTests",
            syntaxTrees: [syntaxTree],
            references: References,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(new StrongTypeGenerator());

        return driver.RunGenerators(compilation);
    }

    private static IReadOnlyList<MetadataReference> BuildReferences()
    {
        var trustedAssemblies = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator);

        var references = trustedAssemblies
            .Select(path => (MetadataReference)MetadataReference.CreateFromFile(path))
            .ToList();

        references.Add(MetadataReference.CreateFromFile(typeof(StrongType).Assembly.Location));

        return references;
    }
}
