namespace SuperStrong.Types.Tests;

public sealed class DateTimePrimitiveData : TheoryData<DateTime>
{
    public DateTimePrimitiveData()
    {
        Add(DateTime.MinValue);
        Add(new DateTime(2024, 1, 2, 3, 4, 5));
    }
}
