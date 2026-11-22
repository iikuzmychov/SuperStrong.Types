using DiffEngine;
using SuperStrong.Types.CodeAnalysis.Generators;
using System.Runtime.CompilerServices;
using System.Text;

namespace SuperStrong.Types.CodeAnalysis.Tests;

internal static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
        VerifierSettings.AddScrubber(ScrubGeneratedCodeVersion);
        DiffRunner.Disabled = true;
    }

    private static void ScrubGeneratedCodeVersion(StringBuilder builder)
    {
        var generator = typeof(StrongTypeGenerator).Assembly.GetName();

        builder.Replace(
            $$"""GeneratedCodeAttribute("{{generator.Name}}", "{{generator.Version}}")""",
            $$"""GeneratedCodeAttribute("{{generator.Name}}", "{Version}")""");
    }
}
