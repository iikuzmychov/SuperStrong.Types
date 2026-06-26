namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class FloatStrongTypeConverterTests
    : StrongTypeConverterTests<
        FloatStrongTypeConverterTests.StrongFloat,
        float,
        FloatStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<float>]
    public sealed partial class StrongFloat;

    public sealed class PrimitiveTheoryData : TheoryData<float>
    {
        public PrimitiveTheoryData()
        {
            Add(float.MinValue);
            Add(-1.5f);
            Add(0f);
            Add(float.MaxValue);
        }
    }
}
