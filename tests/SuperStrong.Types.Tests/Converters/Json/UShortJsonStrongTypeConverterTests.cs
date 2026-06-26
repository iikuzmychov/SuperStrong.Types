namespace SuperStrong.Types.Tests.Converters.Json;

public sealed partial class UShortJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        UShortJsonStrongTypeConverterTests.StrongUShort,
        ushort,
        UShortJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<ushort>]
    public sealed partial class StrongUShort;

    public sealed class PrimitiveData : TheoryData<ushort>
    {
        public PrimitiveData()
        {
            Add(0);
            Add(ushort.MaxValue);
        }
    }
}
