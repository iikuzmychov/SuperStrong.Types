namespace SuperStrong.Types.Tests;

[StrongType<DateOnly>]
public sealed partial class StrongDateOnly
{
    public static readonly DateOnly ForbiddenValue = new DateOnly(1234, 5, 6);

    public static StrongTypeDefinition<DateOnly> Definition { get; } = StrongType.Define<DateOnly>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<DateOnly>
    {
        public ValidPrimitiveSamples()
        {
            Add(DateOnly.MinValue);
            Add(new DateOnly(2024, 1, 2));
            Add(DateOnly.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<DateOnly>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
