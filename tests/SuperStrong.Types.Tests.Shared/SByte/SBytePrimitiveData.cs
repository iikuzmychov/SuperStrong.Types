namespace SuperStrong.Types.Tests;

public sealed class SBytePrimitiveData : TheoryData<sbyte>
{
    public SBytePrimitiveData()
    {
        Add(sbyte.MinValue);
        Add(0);
        Add(sbyte.MaxValue);
    }
}
