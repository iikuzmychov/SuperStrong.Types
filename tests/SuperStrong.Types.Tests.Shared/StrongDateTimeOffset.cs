namespace SuperStrong.Types.Tests;

[StrongType<DateTimeOffset>]
public sealed partial class StrongDateTimeOffset
{
    public static readonly DateTimeOffset ForbiddenValue = new DateTimeOffset(1234, 5, 6, 7, 8, 9, TimeSpan.FromHours(1));

    public static StrongTypeDefinition<DateTimeOffset> Define() => StrongType.Define<DateTimeOffset>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<DateTimeOffset>
    {
        public ValidPrimitiveSamples()
        {
            Add(DateTimeOffset.MinValue);
            Add(new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.FromHours(2)));
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<DateTimeOffset>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
