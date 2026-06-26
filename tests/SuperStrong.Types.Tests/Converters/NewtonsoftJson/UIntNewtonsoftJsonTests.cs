namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class UIntNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        UIntNewtonsoftJsonTests.StrongUInt,
        uint,
        UIntNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<uint>]
    public sealed partial class StrongUInt;

    public sealed class PrimitiveData : TheoryData<uint>
    {
        public PrimitiveData()
        {
            Add(0);
            Add(uint.MaxValue);
        }
    }
}
