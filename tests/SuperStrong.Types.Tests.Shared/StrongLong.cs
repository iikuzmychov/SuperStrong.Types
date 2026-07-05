namespace SuperStrong.Types.Tests;

[StrongType<long>]
public sealed partial class StrongLong
{
    public static readonly long ForbiddenValue = 1_234_567_890_123_456_789;

    public static StrongTypeDefinition<long> Definition { get; } = StrongType.Define<long>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<long>
    {
        public ValidPrimitiveSamples()
        {
            Add(long.MinValue);
            Add(0);
            Add(long.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<long>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
