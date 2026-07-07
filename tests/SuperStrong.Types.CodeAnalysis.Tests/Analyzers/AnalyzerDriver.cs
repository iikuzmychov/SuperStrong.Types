using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using SuperStrong.Types.CodeAnalysis.Generators;

namespace SuperStrong.Types.CodeAnalysis.Tests.Analyzers;

internal static class AnalyzerDriver
{
    public static Task<IReadOnlyList<Diagnostic>> GetDiagnosticsAsync(DiagnosticAnalyzer analyzer, string source)
    {
        return GetDiagnosticsAsync(analyzer, TestWorkspace.CreateDocument(new StrongTypeGenerator(), source));
    }

    public static async Task<IReadOnlyList<Diagnostic>> GetDiagnosticsAsync(DiagnosticAnalyzer analyzer, Document document)
    {
        var compilation = await document.Project.GetCompilationAsync();

        if (compilation is null)
        {
            return [];
        }

        return await compilation.WithAnalyzers([analyzer]).GetAnalyzerDiagnosticsAsync();
    }
}
