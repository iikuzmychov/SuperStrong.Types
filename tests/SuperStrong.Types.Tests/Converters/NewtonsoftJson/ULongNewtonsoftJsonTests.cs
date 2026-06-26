namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class ULongNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        ULongNewtonsoftJsonTests.StrongULong,
        ulong,
        ULongNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<ulong>]
    public sealed partial class StrongULong;

    public sealed class PrimitiveData : TheoryData<ulong>
    {
        public PrimitiveData()
        {
            Add(0);
            Add(ulong.MaxValue);
        }
    }
}
