namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class LongNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        LongNewtonsoftJsonTests.StrongLong,
        long,
        LongNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<long>]
    public sealed partial class StrongLong;

    public sealed class PrimitiveData : TheoryData<long>
    {
        public PrimitiveData()
        {
            Add(long.MinValue);
            Add(0);
            Add(long.MaxValue);
        }
    }
}
