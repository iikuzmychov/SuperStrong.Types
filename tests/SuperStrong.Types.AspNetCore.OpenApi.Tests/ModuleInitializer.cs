using DiffEngine;
using System.Runtime.CompilerServices;

namespace SuperStrong.Types.AspNetCore.OpenApi.Tests;

internal static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        DiffRunner.Disabled = true;
    }
}
