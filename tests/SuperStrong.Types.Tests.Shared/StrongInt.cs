namespace SuperStrong.Types.Tests;

[StrongType<int>]
public sealed partial class StrongInt
{
    public static readonly int ForbiddenValue = 123_456_789;

    public static partial StrongTypeDefinition<int> Define() => StrongType.Define<int>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<int>
    {
        public ValidPrimitiveSamples()
        {
            Add(int.MinValue);
            Add(0);
            Add(int.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<int>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
