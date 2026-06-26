namespace SuperStrong.Types.Tests.Converters.Json;

public sealed partial class ShortJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        ShortJsonStrongTypeConverterTests.StrongShort,
        short,
        ShortJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<short>]
    public sealed partial class StrongShort;

    public sealed class PrimitiveData : TheoryData<short>
    {
        public PrimitiveData()
        {
            Add(short.MinValue);
            Add(0);
            Add(short.MaxValue);
        }
    }
}
