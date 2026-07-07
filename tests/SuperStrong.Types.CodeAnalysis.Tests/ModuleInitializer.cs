using DiffEngine;
using System.Runtime.CompilerServices;

namespace SuperStrong.Types.CodeAnalysis.Tests;

internal static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
        DiffRunner.Disabled = true;
    }
}
