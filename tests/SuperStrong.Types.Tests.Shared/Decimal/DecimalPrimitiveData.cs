namespace SuperStrong.Types.Tests;

public sealed class DecimalPrimitiveData : TheoryData<decimal>
{
    public DecimalPrimitiveData()
    {
        Add(decimal.MinValue);
        Add(-1.5m);
        Add(0m);
        Add(decimal.MaxValue);
    }
}
