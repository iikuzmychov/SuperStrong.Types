namespace SuperStrong.Types.Tests.Converters.Json;

public sealed partial class FloatJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        FloatJsonStrongTypeConverterTests.StrongFloat,
        float,
        FloatJsonStrongTypeConverterTests.PrimitiveData>
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
