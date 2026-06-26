namespace SuperStrong.Types.Tests.Converters.NewtonsoftJson;

public sealed partial class TimeOnlyNewtonsoftJsonTests
    : NewtonsoftJsonStrongTypeTests<
        TimeOnlyNewtonsoftJsonTests.StrongTimeOnly,
        TimeOnly,
        TimeOnlyNewtonsoftJsonTests.PrimitiveData>
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
