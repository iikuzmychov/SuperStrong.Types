namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class StringNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        StringNewtonsoftJsonTests.StrongString,
        string,
        StringNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<string>]
    public sealed partial class StrongString;

    public sealed class PrimitiveData : TheoryData<string>
    {
        public PrimitiveData()
        {
            Add("hello");
            Add("");
            Add(" ");
            Add("a\"b\\c");
            Add("café");
        }
    }
}
