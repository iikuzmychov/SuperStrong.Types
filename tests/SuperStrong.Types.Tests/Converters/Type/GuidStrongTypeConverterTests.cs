namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class GuidStrongTypeConverterTests
    : StrongTypeConverterTests<
        GuidStrongTypeConverterTests.StrongGuid,
        Guid,
        GuidStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<Guid>]
    public sealed partial class StrongGuid;

    public sealed class PrimitiveTheoryData : TheoryData<Guid>
    {
        public PrimitiveTheoryData()
        {
            Add(Guid.Empty);
            Add(Guid.Parse("12345678-1234-1234-1234-1234567890ab"));
        }
    }
}
