namespace SuperStrong.Types.Tests;

[StrongType<double>]
public sealed partial class StrongDouble
{
    public static readonly double ForbiddenValue = 123_456_789.123;

    public static StrongTypeDefinition<double> Define() => StrongType.Define<double>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<double>
    {
        public ValidPrimitiveSamples()
        {
            Add(double.MinValue);
            Add(-1.5);
            Add(0d);
            Add(double.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<double>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
