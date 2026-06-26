namespace SuperStrong.Types.Tests;

public sealed class UShortPrimitiveData : TheoryData<ushort>
{
    public UShortPrimitiveData()
    {
        Add(0);
        Add(ushort.MaxValue);
    }
}
