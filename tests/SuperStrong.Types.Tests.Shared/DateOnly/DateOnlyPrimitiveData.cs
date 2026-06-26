namespace SuperStrong.Types.Tests;

public sealed class DateOnlyPrimitiveData : TheoryData<DateOnly>
{
    public DateOnlyPrimitiveData()
    {
        Add(DateOnly.MinValue);
        Add(new DateOnly(2024, 1, 2));
        Add(DateOnly.MaxValue);
    }
}
