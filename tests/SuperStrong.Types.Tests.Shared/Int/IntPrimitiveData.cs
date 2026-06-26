namespace SuperStrong.Types.Tests;

public sealed class IntPrimitiveData : TheoryData<int>
{
    public IntPrimitiveData()
    {
        Add(int.MinValue);
        Add(0);
        Add(int.MaxValue);
    }
}
