namespace SuperStrong.Types.Tests;

[StrongType<uint>]
public sealed partial class StrongUInt
{
    public static readonly uint ForbiddenValue = 123_456_789;

    public static StrongTypeDefinition<uint> Definition { get; } = StrongType.Define<uint>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<uint>
    {
        public ValidPrimitiveSamples()
        {
            Add(0);
            Add(uint.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<uint>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
