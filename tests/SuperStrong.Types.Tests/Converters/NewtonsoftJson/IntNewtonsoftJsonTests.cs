namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class IntNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        IntNewtonsoftJsonTests.StrongInt,
        int,
        IntNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<int>]
    public sealed partial class StrongInt;

    public sealed class PrimitiveData : TheoryData<int>
    {
        public PrimitiveData()
        {
            Add(int.MinValue);
            Add(0);
            Add(int.MaxValue);
        }
    }
}
