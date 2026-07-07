namespace SuperStrong.Types.Tests;

[StrongType<float>]
public sealed partial class StrongFloat
{
    public static readonly float ForbiddenValue = 123_456.75f;

    public static partial StrongTypeDefinition<float> Define() => StrongType.Define<float>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<float>
    {
        public ValidPrimitiveSamples()
        {
            Add(float.MinValue);
            Add(-1.5f);
            Add(0f);
            Add(float.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<float>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
