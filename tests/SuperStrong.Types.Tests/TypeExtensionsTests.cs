using SuperStrong.Types.Reflection;

namespace SuperStrong.Types.Tests;

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
    [InlineData(typeof(StrongIntStruct?))]
    [InlineData(typeof(StrongTemplatedIntStruct?))]
    public void GetStrongTypeInfo_returns_null_for_non_strong_type(Type type)
    {
        Assert.Null(type.GetStrongTypeInfo());
    }

    public static TheoryData<Type, Type, Type?, StrongTypeDefinition> StrongTypeFixtures => new()
    {
        { typeof(StrongIntClass), typeof(int), null, StrongIntClass.Definition },
        { typeof(StrongIntStruct), typeof(int), null, StrongIntStruct.Definition },
        { typeof(StrongStringClass), typeof(string), null, StrongStringClass.Definition },
        { typeof(StrongTemplatedIntClass), typeof(int), typeof(StrongIntTemplate), StrongTemplatedIntClass.Definition },
        { typeof(StrongTemplatedIntStruct), typeof(int), typeof(StrongIntTemplate), StrongTemplatedIntStruct.Definition },
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
        Assert.Equal(type, info.StrongType);
        Assert.Equal(expectedPrimitiveType, info.PrimitiveType);
        Assert.Equal(expectedTemplateType, info.TemplateType);
        Assert.Equivalent(expectedDefinition, info.Definition, strict: true);
    }

    private sealed class NotAStrongType;

    [StrongType<int>]
    private sealed partial class StrongIntClass : IHasStrongTypeDefinition<int>, IHasStrongTypeLayout<int>
    {
        public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMinValue(1);
        public static StrongTypeLayout<int> Layout => StrongType.Layout<int>();
    }

    [StrongType<int>]
    private readonly partial struct StrongIntStruct : IHasStrongTypeDefinition<int>, IHasStrongTypeLayout<int>
    {
        public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMinValue(2);
        public static StrongTypeLayout<int> Layout => StrongType.Layout<int>();
    }

    [StrongType<string>]
    private sealed partial class StrongStringClass : IHasStrongTypeDefinition<string>, IHasStrongTypeLayout<string>
    {
        public static StrongTypeDefinition<string> Definition => StrongType.Define<string>().HasMinLength(1);
        public static StrongTypeLayout<string> Layout => StrongType.Layout<string>();
    }

    [StrongType<int, StrongIntTemplate>]
    private sealed partial class StrongTemplatedIntClass : IHasStrongTypeDefinition<int>, IHasStrongTypeLayout<int>
    {
        public static StrongTypeDefinition<int> Definition => StrongIntTemplate.Definition;
        public static StrongTypeLayout<int> Layout => StrongType.Layout<int>();
    }

    [StrongType<int, StrongIntTemplate>]
    private readonly partial struct StrongTemplatedIntStruct : IHasStrongTypeDefinition<int>, IHasStrongTypeLayout<int>
    {
        public static StrongTypeDefinition<int> Definition => StrongIntTemplate.Definition;
        public static StrongTypeLayout<int> Layout => StrongType.Layout<int>();
    }

    private sealed class StrongStringTemplate : IStrongTypeTemplate<string>
    {
        public static StrongTypeDefinition<string> Definition => StrongType.Define<string>();
        public static StrongTypeLayout<string> Layout => StrongType.Layout<string>();
    }

    private sealed class StrongIntTemplate : IStrongTypeTemplate<int>
    {
        public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMinValue(3);
        public static StrongTypeLayout<int> Layout => StrongType.Layout<int>();
    }
}
