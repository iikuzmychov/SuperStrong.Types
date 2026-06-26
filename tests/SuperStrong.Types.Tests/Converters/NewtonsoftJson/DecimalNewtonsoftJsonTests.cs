namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class DecimalNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        DecimalNewtonsoftJsonTests.StrongDecimal,
        decimal,
        DecimalNewtonsoftJsonTests.PrimitiveData>
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
