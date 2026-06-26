namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class UIntStrongTypeConverterTests
    : StrongTypeConverterTests<
        UIntStrongTypeConverterTests.StrongUInt,
        uint,
        UIntStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<uint>]
    public sealed partial class StrongUInt;

    public sealed class PrimitiveTheoryData : TheoryData<uint>
    {
        public PrimitiveTheoryData()
        {
            Add(0);
            Add(uint.MaxValue);
        }
    }
}
