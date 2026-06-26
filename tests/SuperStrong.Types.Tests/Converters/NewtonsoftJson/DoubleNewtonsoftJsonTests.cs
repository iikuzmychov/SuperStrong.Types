namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class DoubleNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        DoubleNewtonsoftJsonTests.StrongDouble,
        double,
        DoubleNewtonsoftJsonTests.PrimitiveData>
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
