namespace SuperStrong.Types.Tests;

public sealed class UIntPrimitiveData : TheoryData<uint>
{
    public UIntPrimitiveData()
    {
        Add(0);
        Add(uint.MaxValue);
    }
}
