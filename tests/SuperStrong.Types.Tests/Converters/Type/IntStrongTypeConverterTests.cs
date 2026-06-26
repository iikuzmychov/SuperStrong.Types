namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class IntStrongTypeConverterTests
    : StrongTypeConverterTests<
        IntStrongTypeConverterTests.StrongInt,
        int,
        IntStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<int>]
    public sealed partial class StrongInt;

    public sealed class PrimitiveTheoryData : TheoryData<int>
    {
        public PrimitiveTheoryData()
        {
            Add(int.MinValue);
            Add(0);
            Add(int.MaxValue);
        }
    }
}
