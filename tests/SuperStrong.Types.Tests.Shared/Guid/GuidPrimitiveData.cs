namespace SuperStrong.Types.Tests;

public sealed class GuidPrimitiveData : TheoryData<Guid>
{
    public GuidPrimitiveData()
    {
        Add(Guid.Empty);
        Add(Guid.Parse("12345678-1234-1234-1234-1234567890ab"));
    }
}
