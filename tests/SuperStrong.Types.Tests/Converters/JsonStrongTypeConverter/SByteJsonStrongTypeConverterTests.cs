namespace SuperStrong.Types.Tests.Converters;

public sealed partial class SByteJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        SByteJsonStrongTypeConverterTests.StrongSByte,
        sbyte,
        SByteJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<sbyte>]
    public sealed partial class StrongSByte;

    public sealed class PrimitiveData : TheoryData<sbyte>
    {
        public PrimitiveData()
        {
            Add(sbyte.MinValue);
            Add(0);
            Add(sbyte.MaxValue);
        }
    }
}
