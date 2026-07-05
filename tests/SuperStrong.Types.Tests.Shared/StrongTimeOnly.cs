namespace SuperStrong.Types.Tests;

[StrongType<TimeOnly>]
public sealed partial class StrongTimeOnly
{
    public static readonly TimeOnly ForbiddenValue = new TimeOnly(1, 23, 45);

    public static StrongTypeDefinition<TimeOnly> Definition { get; } = StrongType.Define<TimeOnly>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<TimeOnly>
    {
        public ValidPrimitiveSamples()
        {
            Add(TimeOnly.MinValue);
            Add(new TimeOnly(3, 4, 5));
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<TimeOnly>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
