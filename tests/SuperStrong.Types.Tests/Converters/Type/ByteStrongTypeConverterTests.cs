namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class ByteStrongTypeConverterTests
    : StrongTypeConverterTests<
        ByteStrongTypeConverterTests.StrongByte,
        byte,
        ByteStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<byte>]
    public sealed partial class StrongByte;

    public sealed class PrimitiveTheoryData : TheoryData<byte>
    {
        public PrimitiveTheoryData()
        {
            Add(0);
            Add(byte.MaxValue);
        }
    }
}
