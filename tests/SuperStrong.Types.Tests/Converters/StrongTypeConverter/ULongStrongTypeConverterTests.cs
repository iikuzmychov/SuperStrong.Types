namespace SuperStrong.Types.Tests.Converters;

public sealed partial class ULongStrongTypeConverterTests
    : StrongTypeConverterTests<
        ULongStrongTypeConverterTests.StrongULong,
        ulong,
        ULongStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<ulong>]
    public sealed partial class StrongULong;

    public sealed class PrimitiveTheoryData : TheoryData<ulong>
    {
        public PrimitiveTheoryData()
        {
            Add(0);
            Add(ulong.MaxValue);
        }
    }
}
