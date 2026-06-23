namespace SuperStrong.Types.Tests.Converters;

public sealed partial class DoubleJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        DoubleJsonStrongTypeConverterTests.StrongDouble,
        double,
        DoubleJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<double>]
    public sealed partial class StrongDouble;

    public sealed class PrimitiveData : TheoryData<double>
    {
        public PrimitiveData()
        {
            Add(double.MinValue);
            Add(-1.5);
            Add(0d);
            Add(double.MaxValue);
        }
    }
}
