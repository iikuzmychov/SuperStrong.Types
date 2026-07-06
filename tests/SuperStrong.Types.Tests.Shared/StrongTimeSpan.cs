namespace SuperStrong.Types.Tests;

[StrongType<TimeSpan>]
public sealed partial class StrongTimeSpan
{
    public static readonly TimeSpan ForbiddenValue = new TimeSpan(12, 34, 56);

    public static StrongTypeDefinition<TimeSpan> Define() => StrongType.Define<TimeSpan>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<TimeSpan>
    {
        public ValidPrimitiveSamples()
        {
            Add(TimeSpan.Zero);
            Add(new TimeSpan(1, 2, 3));
            Add(new TimeSpan(-1, -2, -3));
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<TimeSpan>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
