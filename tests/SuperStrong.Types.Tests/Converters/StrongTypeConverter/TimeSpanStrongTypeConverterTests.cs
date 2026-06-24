namespace SuperStrong.Types.Tests.Converters;

public sealed partial class TimeSpanStrongTypeConverterTests
    : StrongTypeConverterTests<
        TimeSpanStrongTypeConverterTests.StrongTimeSpan,
        TimeSpan,
        TimeSpanStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<TimeSpan>]
    public sealed partial class StrongTimeSpan;

    public sealed class PrimitiveTheoryData : TheoryData<TimeSpan>
    {
        public PrimitiveTheoryData()
        {
            Add(TimeSpan.Zero);
            Add(new TimeSpan(1, 2, 3));
            Add(new TimeSpan(-1, -2, -3));
        }
    }
}
