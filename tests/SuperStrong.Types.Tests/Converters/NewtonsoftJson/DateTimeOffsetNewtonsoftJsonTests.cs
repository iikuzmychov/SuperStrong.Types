namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class DateTimeOffsetNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        DateTimeOffsetNewtonsoftJsonTests.StrongDateTimeOffset,
        DateTimeOffset,
        DateTimeOffsetNewtonsoftJsonTests.PrimitiveData>
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
