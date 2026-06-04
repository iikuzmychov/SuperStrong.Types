using SuperStrong.Types;
using SuperStrong.Types.EntityFrameworkCore.Tests.ExampleTypes;

namespace SuperStrong.Tests.ExampleTypes;

[StrongType<int, StrongTypeIdTemplate>]
public sealed partial class UserId : IHasStrongTypeDefinition<int>, IHasStrongTypeLayout<int>
{
    public static StrongTypeDefinition<int> Definition => StrongTypeIdTemplate.Definition;
    public static StrongTypeLayout<int> Layout => StrongTypeIdTemplate.Layout;
}
