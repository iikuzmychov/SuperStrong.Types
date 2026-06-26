namespace SuperStrong.Types.Tests;

public sealed class ShortPrimitiveData : TheoryData<short>
{
    public ShortPrimitiveData()
    {
        Add(short.MinValue);
        Add(0);
        Add(short.MaxValue);
    }
}
