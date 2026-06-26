namespace SuperStrong.Types.Tests;

public sealed class DateTimeOffsetPrimitiveData : TheoryData<DateTimeOffset>
{
    public DateTimeOffsetPrimitiveData()
    {
        Add(DateTimeOffset.MinValue);
        Add(new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.FromHours(2)));
    }
}
