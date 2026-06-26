namespace SuperStrong.Types.Tests;

public sealed class DoublePrimitiveData : TheoryData<double>
{
    public DoublePrimitiveData()
    {
        Add(double.MinValue);
        Add(-1.5);
        Add(0d);
        Add(double.MaxValue);
    }
}
