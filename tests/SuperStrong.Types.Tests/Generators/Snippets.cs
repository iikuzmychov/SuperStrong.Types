namespace SuperStrong.Types.Tests.Generators;

internal static class Snippets
{
    public static string DisableAllFeatures()
    {
        return string.Join(
            Environment.NewLine,
            Constants.AllFeatures.Select(feature => $"[assembly: StrongTypeFeatures.{feature}(IsEnabled = false)]"));
    }
}
