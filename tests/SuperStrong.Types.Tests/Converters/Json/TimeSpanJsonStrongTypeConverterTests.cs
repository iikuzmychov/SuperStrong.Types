namespace SuperStrong.Types.Tests.Converters.Json;

public sealed partial class TimeSpanJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        TimeSpanJsonStrongTypeConverterTests.StrongTimeSpan,
        TimeSpan,
        TimeSpanJsonStrongTypeConverterTests.PrimitiveData>
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
