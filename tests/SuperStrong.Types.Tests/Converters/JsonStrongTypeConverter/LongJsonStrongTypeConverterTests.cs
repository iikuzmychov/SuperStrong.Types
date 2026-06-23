namespace SuperStrong.Types.Tests.Converters;

public sealed partial class LongJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        LongJsonStrongTypeConverterTests.StrongLong,
        long,
        LongJsonStrongTypeConverterTests.PrimitiveData>
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
