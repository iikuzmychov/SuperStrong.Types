namespace SuperStrong.Types.Tests.Converters;

public sealed partial class UIntJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        UIntJsonStrongTypeConverterTests.StrongUInt,
        uint,
        UIntJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<uint>]
    public sealed partial class StrongUInt;

    public sealed class PrimitiveData : TheoryData<uint>
    {
        public PrimitiveData()
        {
            Add(0);
            Add(uint.MaxValue);
        }
    }
}
