namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class BoolStrongTypeConverterTests
    : StrongTypeConverterTests<
        BoolStrongTypeConverterTests.StrongBool,
        bool,
        BoolStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<bool>]
    public sealed partial class StrongBool;

    public sealed class PrimitiveTheoryData : TheoryData<bool>
    {
        public PrimitiveTheoryData()
        {
            Add(true);
            Add(false);
        }
    }
}
