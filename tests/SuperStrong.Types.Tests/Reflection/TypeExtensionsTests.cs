using SuperStrong.Types.Reflection;

namespace SuperStrong.Types.Tests.Reflection;

public sealed partial class TypeExtensionsTests
{
    [Fact]
    public void GetStrongTypeInfo_throws_for_null()
    {
        Assert.Throws<ArgumentNullException>(() => TypeExtensions.GetStrongTypeInfo(null!));
    }

    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(int?))]
    [InlineData(typeof(string))]
    [InlineData(typeof(NotAStrongType))]
    //[InlineData(typeof(StrongIntStruct?))] // todo: uncomment, once generator will work with structs too
    //[InlineData(typeof(StrongTemplatedIntStruct?))] // todo: uncomment, once generator will work with structs too
    public void GetStrongTypeInfo_returns_null_for_non_strong_type(Type type)
    {
        Assert.Null(type.GetStrongTypeInfo());
    }

    public static TheoryData<Type, Type, Type?, StrongTypeDefinition> StrongTypeFixtures => new()
    {
        { typeof(StrongIntClass), typeof(int), null, StrongType.GetDefinition<StrongIntClass, int>() },
        //{ typeof(StrongIntStruct), typeof(int), null, StrongType.GetDefinition<StrongIntStruct, int>() }, // todo: uncomment, once generator will work with structs too
        { typeof(StrongStringClass), typeof(string), null, StrongType.GetDefinition<StrongStringClass, string>() },
        { typeof(StrongTemplatedIntClass), typeof(int), typeof(StrongIntTemplate), StrongType.GetDefinition<StrongTemplatedIntClass, int>() },
        //{ typeof(StrongTemplatedIntStruct), typeof(int), typeof(StrongIntTemplate), StrongType.GetDefinition<StrongTemplatedIntStruct, int>() }, // todo: uncomment, once generator will work with structs too
    };

    [Theory]
    [MemberData(nameof(StrongTypeFixtures))]
    public void GetStrongTypeInfo_returns_expected_info(
        Type type,
        Type expectedPrimitiveType,
        Type? expectedTemplateType,
        StrongTypeDefinition expectedDefinition)
    {
        var info = type.GetStrongTypeInfo();
        
        Assert.NotNull(info);
        Assert.Equal(type, info.ClrType);
        Assert.Equal(expectedPrimitiveType, info.PrimitiveType);
        Assert.Equal(expectedTemplateType, info.TemplateType);
        Assert.Same(expectedDefinition, info.Definition);
    }

    private sealed class NotAStrongType;

    [StrongType<int>]
    private sealed partial class StrongIntClass
    {
        public static partial StrongTypeDefinition<int> Define() => StrongType.Define<int>().HasMinValue(1);
    }

    // todo: uncomment, once generator will work with structs too
    //[StrongType<int>]
    //private readonly partial struct StrongIntStruct
    //{
    //    public static partial StrongTypeDefinition<int> Define() => StrongType.Define<int>().HasMinValue(2);
    //}

    [StrongType<string>]
    private sealed partial class StrongStringClass
    {
        public static partial StrongTypeDefinition<string> Define() => StrongType.Define<string>().HasMinLength(1);
    }

    [StrongType<int, StrongIntTemplate>]
    private sealed partial class StrongTemplatedIntClass;

    // todo: uncomment, once generator will work with structs too
    //[StrongType<int, StrongIntTemplate>]
    //private readonly partial struct StrongTemplatedIntStruct;

    private sealed class StrongStringTemplate : IStrongTypeTemplate<string>
    {
        public static StrongTypeDefinition<string> Define() => StrongType.Define<string>().HasMinLength(5);
    }

    private sealed class StrongIntTemplate : IStrongTypeTemplate<int>
    {
        public static StrongTypeDefinition<int> Define() => StrongType.Define<int>().HasMinValue(3);
    }
}
