namespace SuperStrong.Types.Tests;

public sealed class LongPrimitiveData : TheoryData<long>
{
    public LongPrimitiveData()
    {
        Add(long.MinValue);
        Add(0);
        Add(long.MaxValue);
    }
}
