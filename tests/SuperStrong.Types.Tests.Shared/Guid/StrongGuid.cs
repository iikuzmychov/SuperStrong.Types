namespace SuperStrong.Types.Tests;

[StrongType<Guid>]
public sealed partial class StrongGuid
{
    public static readonly Guid ForbiddenValue = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");

    public static StrongTypeDefinition<Guid> Definition { get; } = StrongType.Define<Guid>().IsNot(ForbiddenValue);

    public sealed class ValidPrimitiveSamples : TheoryData<Guid>
    {
        public ValidPrimitiveSamples()
        {
            Add(Guid.Empty);
            Add(Guid.Parse("12345678-1234-1234-1234-1234567890ab"));
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<Guid>
    {
        public InvalidPrimitiveSamples()
        {
            Add(ForbiddenValue);
        }
    }
}
