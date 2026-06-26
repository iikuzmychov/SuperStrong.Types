namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class BoolNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        BoolNewtonsoftJsonTests.StrongBool,
        bool,
        BoolNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<bool>]
    public sealed partial class StrongBool;

    public sealed class PrimitiveData : TheoryData<bool>
    {
        public PrimitiveData()
        {
            Add(true);
            Add(false);
        }
    }
}
