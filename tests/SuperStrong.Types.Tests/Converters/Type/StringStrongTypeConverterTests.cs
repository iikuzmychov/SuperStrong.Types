namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class StringStrongTypeConverterTests
    : StrongTypeConverterTests<
        StringStrongTypeConverterTests.StrongString,
        string,
        StringStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<string>]
    public sealed partial class StrongString;

    public sealed class PrimitiveTheoryData : TheoryData<string>
    {
        public PrimitiveTheoryData()
        {
            Add("hello");
            Add("");
            Add(" ");
            Add("a\"b\\c");
            Add("café");
        }
    }
}
