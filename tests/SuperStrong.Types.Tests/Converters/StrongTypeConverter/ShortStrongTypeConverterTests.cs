namespace SuperStrong.Types.Tests.Converters;

public sealed partial class ShortStrongTypeConverterTests
    : StrongTypeConverterTests<
        ShortStrongTypeConverterTests.StrongShort,
        short,
        ShortStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<short>]
    public sealed partial class StrongShort;

    public sealed class PrimitiveTheoryData : TheoryData<short>
    {
        public PrimitiveTheoryData()
        {
            Add(short.MinValue);
            Add(0);
            Add(short.MaxValue);
        }
    }
}
