namespace SuperStrong.Types.Tests.Converters;

public sealed partial class ByteJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        ByteJsonStrongTypeConverterTests.StrongByte,
        byte,
        ByteJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<byte>]
    public sealed partial class StrongByte;

    public sealed class PrimitiveData : TheoryData<byte>
    {
        public PrimitiveData()
        {
            Add(0);
            Add(byte.MaxValue);
        }
    }
}
