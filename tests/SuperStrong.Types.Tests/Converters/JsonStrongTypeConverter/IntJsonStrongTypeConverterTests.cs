namespace SuperStrong.Types.Tests.Converters;

public sealed partial class IntJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        IntJsonStrongTypeConverterTests.StrongInt,
        int,
        IntJsonStrongTypeConverterTests.PrimitiveData>
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
