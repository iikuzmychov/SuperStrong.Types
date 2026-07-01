using DiffEngine;
using System.Runtime.CompilerServices;

namespace SuperStrong.Types.AspNetCore.Tests;

internal static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        DiffRunner.Disabled = true;
    }
}
