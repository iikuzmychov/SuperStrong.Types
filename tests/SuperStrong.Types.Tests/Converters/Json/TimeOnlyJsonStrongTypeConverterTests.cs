namespace SuperStrong.Types.Tests.Converters.Json;

public sealed partial class TimeOnlyJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<
        TimeOnlyJsonStrongTypeConverterTests.StrongTimeOnly,
        TimeOnly,
        TimeOnlyJsonStrongTypeConverterTests.PrimitiveData>
{
    [StrongType<TimeOnly>]
    public sealed partial class StrongTimeOnly;

    public sealed class PrimitiveData : TheoryData<TimeOnly>
    {
        public PrimitiveData()
        {
            Add(TimeOnly.MinValue);
            Add(new TimeOnly(3, 4, 5));
        }
    }
}
