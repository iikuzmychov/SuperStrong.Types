namespace SuperStrong.Types.Tests.Converters;

public sealed partial class TimeOnlyStrongTypeConverterTests
    : StrongTypeConverterTests<
        TimeOnlyStrongTypeConverterTests.StrongTimeOnly,
        TimeOnly,
        TimeOnlyStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<TimeOnly>]
    public sealed partial class StrongTimeOnly;

    public sealed class PrimitiveTheoryData : TheoryData<TimeOnly>
    {
        public PrimitiveTheoryData()
        {
            Add(TimeOnly.MinValue);
            Add(new TimeOnly(23, 59));
        }
    }
}
