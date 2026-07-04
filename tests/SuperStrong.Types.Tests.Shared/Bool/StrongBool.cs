namespace SuperStrong.Types.Tests;

[StrongType<bool>]
public sealed partial class StrongBool
{
    public sealed class ValidPrimitiveSamples : TheoryData<bool>
    {
        public ValidPrimitiveSamples()
        {
            Add(true);
            Add(false);
        }
    }

    public sealed class InvalidPrimitiveSamples : TheoryData<bool>;
}
