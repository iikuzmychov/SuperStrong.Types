namespace SuperStrong.Types.Tests.Converters;

public sealed partial class DateOnlyJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        DateOnlyJsonStrongTypeConverterTests.StrongDateOnly,
        DateOnly,
        DateOnlyJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<DateOnly>]
    public sealed partial class StrongDateOnly;

    public sealed class PrimitiveData : TheoryData<DateOnly>
    {
        public PrimitiveData()
        {
            Add(DateOnly.MinValue);
            Add(new DateOnly(2024, 1, 2));
            Add(DateOnly.MaxValue);
        }
    }
}
