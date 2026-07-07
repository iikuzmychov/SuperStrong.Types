using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace SuperStrong.Types.CodeAnalysis.Tests;

internal static class TestWorkspace
{
    public const LanguageVersion CSharpVersion = LanguageVersion.CSharp14;

    public static readonly ImmutableArray<MetadataReference> References =
        ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Append(typeof(StrongType).Assembly.Location)
            .Select(path => (MetadataReference)MetadataReference.CreateFromFile(path))
            .ToImmutableArray();

    public static Document CreateDocument(IIncrementalGenerator generator, string source)
    {
        var workspace = new AdhocWorkspace();
        var projectId = ProjectId.CreateNewId();
        var documentId = DocumentId.CreateNewId(projectId);

        var solution = workspace.CurrentSolution
            .AddProject(projectId, "Test", "Test", LanguageNames.CSharp)
            .WithProjectParseOptions(projectId, new CSharpParseOptions(CSharpVersion))
            .WithProjectCompilationOptions(
                projectId,
                new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    nullableContextOptions: NullableContextOptions.Enable))
            .AddMetadataReferences(projectId, References)
            .AddAnalyzerReference(projectId, new SourceGeneratorReference(generator))
            .AddDocument(documentId, "Test.cs", source);

        return solution.GetDocument(documentId)!;
    }

    private sealed class SourceGeneratorReference(IIncrementalGenerator generator) : AnalyzerReference
    {
        private readonly ImmutableArray<ISourceGenerator> _generators = [generator.AsSourceGenerator()];

        public override string? FullPath => null;
        public override object Id => this;
        public override ImmutableArray<DiagnosticAnalyzer> GetAnalyzers(string language) => [];
        public override ImmutableArray<DiagnosticAnalyzer> GetAnalyzersForAllLanguages() => [];
        public override ImmutableArray<ISourceGenerator> GetGenerators(string language) => _generators;
        public override ImmutableArray<ISourceGenerator> GetGeneratorsForAllLanguages() => _generators;
    }
}
