namespace SuperStrong.Types.Tests.Converters.Type;

public sealed partial class DateOnlyStrongTypeConverterTests
    : StrongTypeConverterTests<
        DateOnlyStrongTypeConverterTests.StrongDateOnly,
        DateOnly,
        DateOnlyStrongTypeConverterTests.PrimitiveTheoryData>
{
    [StrongType<DateOnly>]
    public sealed partial class StrongDateOnly;

    public sealed class PrimitiveTheoryData : TheoryData<DateOnly>
    {
        public PrimitiveTheoryData()
        {
            Add(DateOnly.MinValue);
            Add(new DateOnly(2024, 1, 2));
            Add(DateOnly.MaxValue);
        }
    }
}
