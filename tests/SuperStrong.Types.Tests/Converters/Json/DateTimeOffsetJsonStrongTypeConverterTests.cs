namespace SuperStrong.Types.Tests.Converters.Json;

public sealed partial class DateTimeOffsetJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        DateTimeOffsetJsonStrongTypeConverterTests.StrongDateTimeOffset,
        DateTimeOffset,
        DateTimeOffsetJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<DateTimeOffset>]
    public sealed partial class StrongDateTimeOffset;

    public sealed class PrimitiveData : TheoryData<DateTimeOffset>
    {
        public PrimitiveData()
        {
            Add(DateTimeOffset.MinValue);
            Add(new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.FromHours(2)));
        }
    }
}
