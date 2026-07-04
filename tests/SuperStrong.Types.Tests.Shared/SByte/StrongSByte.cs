namespace SuperStrong.Types.Tests;

[StrongType<sbyte>]
public sealed partial class StrongSByte
{
    public static readonly sbyte ForbiddenValue = -123;

    public static StrongTypeDefinition<sbyte> Definition { get; } = StrongType.Define<sbyte>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<sbyte>
    {
        public ValidPrimitiveSamples()
        {
            Add(sbyte.MinValue);
            Add(0);
            Add(sbyte.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<sbyte>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
