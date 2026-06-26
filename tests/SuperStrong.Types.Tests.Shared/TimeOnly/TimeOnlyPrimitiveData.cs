namespace SuperStrong.Types.Tests;

public sealed class TimeOnlyPrimitiveData : TheoryData<TimeOnly>
{
    public TimeOnlyPrimitiveData()
    {
        Add(TimeOnly.MinValue);
        Add(new TimeOnly(3, 4, 5));
    }
}
