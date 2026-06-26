namespace SuperStrong.Types.Tests;

public sealed class BytePrimitiveData : TheoryData<byte>
{
    public BytePrimitiveData()
    {
        Add(0);
        Add(byte.MaxValue);
    }
}
