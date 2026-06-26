namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class UShortStrongTypeConverterTests
    : StrongTypeConverterTests<
        UShortStrongTypeConverterTests.StrongUShort,
        ushort,
        UShortStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<ushort>]
    public sealed partial class StrongUShort;

    public sealed class PrimitiveTheoryData : TheoryData<ushort>
    {
        public PrimitiveTheoryData()
        {
            Add(0);
            Add(ushort.MaxValue);
        }
    }
}
