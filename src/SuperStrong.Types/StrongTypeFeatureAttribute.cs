namespace SuperStrong.Types;

[AttributeUsage(
    AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct,
    Inherited = false,
    AllowMultiple = false)]
public abstract class StrongTypeFeatureAttribute : Attribute
{
    public required bool IsEnabled { get; init; }

    private protected StrongTypeFeatureAttribute()
    {
    }
}
