namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class DecimalStrongTypeConverterTests
    : StrongTypeConverterTests<
        DecimalStrongTypeConverterTests.StrongDecimal,
        decimal,
        DecimalStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<decimal>]
    public sealed partial class StrongDecimal;

    public sealed class PrimitiveTheoryData : TheoryData<decimal>
    {
        public PrimitiveTheoryData()
        {
            Add(decimal.MinValue);
            Add(-1.5m);
            Add(0m);
            Add(decimal.MaxValue);
        }
    }
}
