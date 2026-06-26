namespace SuperStrong.Types.Tests;

public sealed class ULongPrimitiveData : TheoryData<ulong>
{
    public ULongPrimitiveData()
    {
        Add(0);
        Add(ulong.MaxValue);
    }
}
