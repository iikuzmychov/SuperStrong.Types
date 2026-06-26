namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class UShortNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        UShortNewtonsoftJsonTests.StrongUShort,
        ushort,
        UShortNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<ushort>]
    public sealed partial class StrongUShort;

    public sealed class PrimitiveData : TheoryData<ushort>
    {
        public PrimitiveData()
        {
            Add(0);
            Add(ushort.MaxValue);
        }
    }
}
