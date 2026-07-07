namespace SuperStrong.Types.Tests;

[StrongType<ulong>]
public sealed partial class StrongULong
{
    public static readonly ulong ForbiddenValue = 12_345_678_901_234_567_890;

    public static partial StrongTypeDefinition<ulong> Define() => StrongType.Define<ulong>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<ulong>
    {
        public ValidPrimitiveSamples()
        {
            Add(0);
            Add(ulong.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<ulong>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
