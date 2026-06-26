namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class FloatNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        FloatNewtonsoftJsonTests.StrongFloat,
        float,
        FloatNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<float>]
    public sealed partial class StrongFloat;

    public sealed class PrimitiveData : TheoryData<float>
    {
        public PrimitiveData()
        {
            Add(float.MinValue);
            Add(-1.5f);
            Add(0f);
            Add(float.MaxValue);
        }
    }
}
