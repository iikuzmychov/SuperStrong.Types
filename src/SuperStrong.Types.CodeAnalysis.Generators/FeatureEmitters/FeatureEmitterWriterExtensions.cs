using SuperStrong.Types.CodeAnalysis.Generators.Helpers;
using System.Reflection;

namespace SuperStrong.Types.CodeAnalysis.Generators.FeatureEmitters;

internal static class FeatureEmitterWriterExtensions
{
    private static AssemblyName _assemblyName = typeof(StrongTypeGenerator).Assembly.GetName();

    private static void WriteGeneratedCodeAttribute(this IndentedWriter writer)
    {
        writer.Line($"[{System_CodeDom_Compiler_GeneratedCodeAttribute}(\"{_assemblyName.Name}\", \"{_assemblyName.Version}\")]");
    }

    public static void MemberLine(this IndentedWriter writer, string declaration)
    {
        writer.WriteGeneratedCodeAttribute();
        writer.Line(declaration);
    }

    public static IDisposable MemberBlock(this IndentedWriter writer, string declaration)
    {
        writer.WriteGeneratedCodeAttribute();
        return writer.Block(declaration);
    }
}
