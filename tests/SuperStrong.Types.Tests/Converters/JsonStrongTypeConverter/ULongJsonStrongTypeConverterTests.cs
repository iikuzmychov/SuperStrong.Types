namespace SuperStrong.Types.Tests.Converters;

public sealed partial class ULongJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        ULongJsonStrongTypeConverterTests.StrongULong,
        ulong,
        ULongJsonStrongTypeConverterTests.PrimitiveData>
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
