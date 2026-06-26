namespace SuperStrong.Types.Tests;

public sealed class TimeSpanPrimitiveData : TheoryData<TimeSpan>
{
    public TimeSpanPrimitiveData()
    {
        Add(TimeSpan.Zero);
        Add(new TimeSpan(1, 2, 3));
        Add(new TimeSpan(-1, -2, -3));
    }
}
