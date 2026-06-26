namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class DoubleStrongTypeConverterTests
    : StrongTypeConverterTests<
        DoubleStrongTypeConverterTests.StrongDouble,
        double,
        DoubleStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<double>]
    public sealed partial class StrongDouble;

    public sealed class PrimitiveTheoryData : TheoryData<double>
    {
        public PrimitiveTheoryData()
        {
            Add(double.MinValue);
            Add(-1.5);
            Add(0d);
            Add(double.MaxValue);
        }
    }
}
