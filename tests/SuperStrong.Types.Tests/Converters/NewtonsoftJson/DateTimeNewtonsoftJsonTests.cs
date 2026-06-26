namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class DateTimeNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        DateTimeNewtonsoftJsonTests.StrongDateTime,
        DateTime,
        DateTimeNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<DateTime>]
    public sealed partial class StrongDateTime;

    public sealed class PrimitiveData : TheoryData<DateTime>
    {
        public PrimitiveData()
        {
            Add(DateTime.MinValue);
            Add(new DateTime(2024, 1, 2, 3, 4, 5));
        }
    }
}
