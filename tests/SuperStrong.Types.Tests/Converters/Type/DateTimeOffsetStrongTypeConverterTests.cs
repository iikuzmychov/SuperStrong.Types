namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class DateTimeOffsetStrongTypeConverterTests
    : StrongTypeConverterTests<
        DateTimeOffsetStrongTypeConverterTests.StrongDateTimeOffset,
        DateTimeOffset,
        DateTimeOffsetStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<DateTimeOffset>]
    public sealed partial class StrongDateTimeOffset;

    public sealed class PrimitiveTheoryData : TheoryData<DateTimeOffset>
    {
        public PrimitiveTheoryData()
        {
            Add(DateTimeOffset.MinValue);
            Add(new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.FromHours(2)));
        }
    }
}
