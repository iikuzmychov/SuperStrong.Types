namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class SByteStrongTypeConverterTests
    : StrongTypeConverterTests<
        SByteStrongTypeConverterTests.StrongSByte,
        sbyte,
        SByteStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<sbyte>]
    public sealed partial class StrongSByte;

    public sealed class PrimitiveTheoryData : TheoryData<sbyte>
    {
        public PrimitiveTheoryData()
        {
            Add(sbyte.MinValue);
            Add(0);
            Add(sbyte.MaxValue);
        }
    }
}
