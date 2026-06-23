namespace SuperStrong.Types.Tests.Converters;

public sealed partial class CharJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        CharJsonStrongTypeConverterTests.StrongChar,
        char,
        CharJsonStrongTypeConverterTests.PrimitiveData>
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
