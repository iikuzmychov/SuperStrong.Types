namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class ShortNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        ShortNewtonsoftJsonTests.StrongShort,
        short,
        ShortNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<short>]
    public sealed partial class StrongShort;

    public sealed class PrimitiveData : TheoryData<short>
    {
        public PrimitiveData()
        {
            Add(short.MinValue);
            Add(0);
            Add(short.MaxValue);
        }
    }
}
