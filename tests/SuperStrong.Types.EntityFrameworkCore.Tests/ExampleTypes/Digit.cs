using SuperStrong.Types;

namespace SuperStrong.Tests.ExampleTypes;

[StrongType<int>]
public sealed partial class Digit : IHasStrongTypeDefinition<int>, IHasStrongTypeLayout<int>
{
    public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMinValue(0).HasMaxValue(9);
    public static StrongTypeLayout<int> Layout => StrongType.Layout<int>();
}
