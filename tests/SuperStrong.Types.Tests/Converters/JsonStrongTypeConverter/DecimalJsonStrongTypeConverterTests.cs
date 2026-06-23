namespace SuperStrong.Types.Tests.Converters;

public sealed partial class DecimalJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        DecimalJsonStrongTypeConverterTests.StrongDecimal,
        decimal,
        DecimalJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<decimal>]
    public sealed partial class StrongDecimal;

    public sealed class PrimitiveData : TheoryData<decimal>
    {
        public PrimitiveData()
        {
            Add(decimal.MinValue);
            Add(-1.5m);
            Add(0m);
            Add(decimal.MaxValue);
        }
    }
}
