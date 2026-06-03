namespace SuperStrong.Types.EntityFrameworkCore.Tests.ExampleTypes;

public sealed class StrongTypeIdTemplate : IStrongTypeTemplate<int>
{
    public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMinValue(0);
    public static StrongTypeLayout<int> Layout => StrongType.Layout<int>().HasComparisonOperators(true);

    private StrongTypeIdTemplate()
    {
    }
}

