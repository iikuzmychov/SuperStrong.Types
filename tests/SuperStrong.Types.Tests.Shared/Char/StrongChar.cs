namespace SuperStrong.Types.Tests;

[StrongType<char>]
public sealed partial class StrongChar
{
    public static readonly char ForbiddenValue = '☠';

    public static StrongTypeDefinition<char> Definition { get; } = StrongType.Define<char>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<char>
    {
        public ValidPrimitiveSamples()
        {
            Add('a');
            Add('Z');
            Add(' ');
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<char>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
