namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class CharNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        CharNewtonsoftJsonTests.StrongChar,
        char,
        CharNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<char>]
    public sealed partial class StrongChar;

    public sealed class PrimitiveData : TheoryData<char>
    {
        public PrimitiveData()
        {
            Add('a');
            Add('Z');
            Add(' ');
        }
    }
}
