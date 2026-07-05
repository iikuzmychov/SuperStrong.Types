namespace SuperStrong.Types.Tests;

[StrongType<ushort>]
public sealed partial class StrongUShort
{
    public static readonly ushort ForbiddenValue = 12_345;

    public static StrongTypeDefinition<ushort> Definition { get; } = StrongType.Define<ushort>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<ushort>
    {
        public ValidPrimitiveSamples()
        {
            Add(0);
            Add(ushort.MaxValue);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<ushort>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
