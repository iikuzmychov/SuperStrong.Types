namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class TimeSpanNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        TimeSpanNewtonsoftJsonTests.StrongTimeSpan,
        TimeSpan,
        TimeSpanNewtonsoftJsonTests.PrimitiveData>
{
    [StrongType<TimeSpan>]
    public sealed partial class StrongTimeSpan;

    public sealed class PrimitiveData : TheoryData<TimeSpan>
    {
        public PrimitiveData()
        {
            Add(TimeSpan.Zero);
            Add(new TimeSpan(1, 2, 3));
            Add(new TimeSpan(-1, -2, -3));
        }
    }
}
