namespace SuperStrong.Types.Tests.Converters;

public sealed partial class StringJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        StringJsonStrongTypeConverterTests.StrongString,
        string,
        StringJsonStrongTypeConverterTests.PrimitiveData>
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
