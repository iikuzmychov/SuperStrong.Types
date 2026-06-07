using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;

namespace SuperStrong.Types.Generators.Tests;

internal static class StrongTypeGeneratorDriver
{
    private static readonly ImmutableArray<MetadataReference> _references = BuildReferences();

    public static GeneratorDriver Run(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            source,
            new CSharpParseOptions(LanguageVersion.CSharp14));

        var compilation = CSharpCompilation.Create(
            assemblyName: "GeneratorTests",
            syntaxTrees: [syntaxTree],
            references: _references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(new StrongTypeGenerator());

        return driver.RunGenerators(compilation);
    }

    private static ImmutableArray<MetadataReference> BuildReferences()
    {
        var trustedAssemblyPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
        var strongTypeAsemblyPath = typeof(StrongType).Assembly.Location;

        var references = trustedAssemblyPaths
            .Append(strongTypeAsemblyPath)
            .Select(path => (MetadataReference)MetadataReference.CreateFromFile(path))
            .ToImmutableArray();

        return references;
    }
}
