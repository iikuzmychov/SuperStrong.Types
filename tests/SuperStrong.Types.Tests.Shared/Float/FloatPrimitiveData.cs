namespace SuperStrong.Types.Tests;

public sealed class FloatPrimitiveData : TheoryData<float>
{
    public FloatPrimitiveData()
    {
        Add(float.MinValue);
        Add(-1.5f);
        Add(0f);
        Add(float.MaxValue);
    }
}
