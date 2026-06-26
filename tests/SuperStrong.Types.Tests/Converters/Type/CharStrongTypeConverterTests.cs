namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class CharStrongTypeConverterTests
    : StrongTypeConverterTests<
        CharStrongTypeConverterTests.StrongChar,
        char,
        CharStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<char>]
    public sealed partial class StrongChar;

    public sealed class PrimitiveTheoryData : TheoryData<char>
    {
        public PrimitiveTheoryData()
        {
            Add('a');
            Add('Z');
            Add(' ');
        }
    }
}
