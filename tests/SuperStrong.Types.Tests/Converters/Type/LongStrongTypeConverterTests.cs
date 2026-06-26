namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class LongStrongTypeConverterTests
    : StrongTypeConverterTests<
        LongStrongTypeConverterTests.StrongLong,
        long,
        LongStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<long>]
    public sealed partial class StrongLong;

    public sealed class PrimitiveTheoryData : TheoryData<long>
    {
        public PrimitiveTheoryData()
        {
            Add(long.MinValue);
            Add(0);
            Add(long.MaxValue);
        }
    }
}
