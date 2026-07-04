namespace SuperStrong.Types.Tests;

[StrongType<decimal>]
public sealed partial class StrongDecimal
{
    public static readonly decimal ForbiddenValue = 123_456_789.123m;

    public static StrongTypeDefinition<decimal> Definition { get; } = StrongType.Define<decimal>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<decimal>
    {
        public ValidPrimitiveSamples()
        {
            Add(decimal.MinValue);
            Add(-1.5m);
            Add(0m);
            Add(decimal.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<decimal>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
