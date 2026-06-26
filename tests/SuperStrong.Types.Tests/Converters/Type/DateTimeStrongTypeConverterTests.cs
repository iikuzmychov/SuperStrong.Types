namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class DateTimeStrongTypeConverterTests
    : StrongTypeConverterTests<
        DateTimeStrongTypeConverterTests.StrongDateTime,
        DateTime,
        DateTimeStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<DateTime>]
    public sealed partial class StrongDateTime;

    public sealed class PrimitiveTheoryData : TheoryData<DateTime>
    {
        public PrimitiveTheoryData()
        {
            Add(DateTime.MinValue);
            Add(new DateTime(2024, 1, 2, 3, 4, 5));
        }
    }
}
