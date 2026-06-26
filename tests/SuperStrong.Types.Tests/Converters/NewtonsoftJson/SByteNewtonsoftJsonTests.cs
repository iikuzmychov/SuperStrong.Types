namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class SByteNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        SByteNewtonsoftJsonTests.StrongSByte,
        sbyte,
        SByteNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<sbyte>]
    public sealed partial class StrongSByte;

    public sealed class PrimitiveData : TheoryData<sbyte>
    {
        public PrimitiveData()
        {
            Add(sbyte.MinValue);
            Add(0);
            Add(sbyte.MaxValue);
        }
    }
}
