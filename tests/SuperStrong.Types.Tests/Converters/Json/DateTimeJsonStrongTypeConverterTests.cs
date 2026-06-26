namespace SuperStrong.Types.Tests.Converters.Json;

public sealed partial class DateTimeJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        DateTimeJsonStrongTypeConverterTests.StrongDateTime,
        DateTime,
        DateTimeJsonStrongTypeConverterTests.PrimitiveData>
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
