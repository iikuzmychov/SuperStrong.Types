namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class GuidNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        GuidNewtonsoftJsonTests.StrongGuid,
        Guid,
        GuidNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<Guid>]
    public sealed partial class StrongGuid;

    public sealed class PrimitiveData : TheoryData<Guid>
    {
        public PrimitiveData()
        {
            Add(Guid.Empty);
            Add(Guid.Parse("12345678-1234-1234-1234-1234567890ab"));
        }
    }
}
