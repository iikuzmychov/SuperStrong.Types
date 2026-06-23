namespace SuperStrong.Types.Tests.Converters;

public sealed partial class BoolJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        BoolJsonStrongTypeConverterTests.StrongBool,
        bool,
        BoolJsonStrongTypeConverterTests.PrimitiveData>
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
