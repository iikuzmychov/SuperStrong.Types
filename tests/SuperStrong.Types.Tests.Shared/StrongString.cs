namespace SuperStrong.Types.Tests;

[StrongType<string>]
public sealed partial class StrongString
{
    public static readonly string ForbiddenValue = "!forbidden!";

    public static partial StrongTypeDefinition<string> Define() => StrongType.Define<string>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<string>
    {
        public ValidPrimitiveSamples()
        {
            Add("hello");
            Add("");
            Add(" ");
            Add("a\"b\\c");
            Add("café");
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<string>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
