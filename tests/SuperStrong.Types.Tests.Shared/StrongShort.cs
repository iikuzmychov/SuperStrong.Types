namespace SuperStrong.Types.Tests;

[StrongType<short>]
public sealed partial class StrongShort
{
    public static readonly short ForbiddenValue = -12_345;

    public static StrongTypeDefinition<short> Define() => StrongType.Define<short>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<short>
    {
        public ValidPrimitiveSamples()
        {
            Add(short.MinValue);
            Add(0);
            Add(short.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<short>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
