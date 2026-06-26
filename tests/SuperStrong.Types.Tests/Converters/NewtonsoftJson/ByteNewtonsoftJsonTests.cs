namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class ByteNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        ByteNewtonsoftJsonTests.StrongByte,
        byte,
        ByteNewtonsoftJsonTests.PrimitiveData>
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
