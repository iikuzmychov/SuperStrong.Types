namespace SuperStrong.Types.Tests;

public sealed class BoolPrimitiveData : TheoryData<bool>
{
    public BoolPrimitiveData()
    {
        Add(true);
        Add(false);
    }
}
