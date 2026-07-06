namespace SuperStrong.Types.Tests;

[StrongType<byte>]
public sealed partial class StrongByte
{
    public static readonly byte ForbiddenValue = 123;

    public static StrongTypeDefinition<byte> Define() => StrongType.Define<byte>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<byte>
    {
        public ValidPrimitiveSamples()
        {
            Add(0);
            Add(byte.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<byte>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
