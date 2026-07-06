namespace SuperStrong.Types.Tests;

[StrongType<DateTime>]
public sealed partial class StrongDateTime
{
    public static readonly DateTime ForbiddenValue = new DateTime(1234, 5, 6, 7, 8, 9);

    public static StrongTypeDefinition<DateTime> Define() => StrongType.Define<DateTime>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<DateTime>
    {
        public ValidPrimitiveSamples()
        {
            Add(DateTime.MinValue);
            Add(new DateTime(2024, 1, 2, 3, 4, 5));
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<DateTime>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
