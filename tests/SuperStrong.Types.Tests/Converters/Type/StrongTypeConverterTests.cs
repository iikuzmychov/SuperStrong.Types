#pragma warning disable xUnit1019

using System.ComponentModel;
using System.Globalization;
using SuperStrong.Types.Converters;

namespace SuperStrong.Types.Tests.Converters;

public abstract class StrongTypeConverterTests<TStrongType, TPrimitive, TPrimitivesData>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TPrimitivesData : notnull, TheoryData<TPrimitive>, new()
{
    private static readonly TypeConverter _converter = TypeDescriptor.GetConverter(typeof(TStrongType));
    private static readonly TypeConverter _primitiveConverter = TypeDescriptor.GetConverter(typeof(TPrimitive));

    public static TheoryData<TPrimitive> PrimitivesData { get; } = new TPrimitivesData();
    public static TheoryData<TStrongType> StrongTypesData { get; } = CreateStrongTypesData();
    public static TheoryData<string> StringsData { get; } = CreateStringsData();

    private static TheoryData<TStrongType> CreateStrongTypesData()
    {
        return new(PrimitivesData.Select(primitive => TStrongType.From(primitive)));
    }

    private static TheoryData<string> CreateStringsData()
    {
        return new(
            PrimitivesData.Select(primitive =>
                (string)_primitiveConverter.ConvertTo(null, CultureInfo.InvariantCulture, (TPrimitive)primitive, typeof(string))!));
    }

    [Fact]
    public void Constructor_throws_for_null()
    {
        Assert.Throws<ArgumentNullException>(() => new StrongTypeConverter(null!));
    }

    [Fact]
    public void Constructor_throws_for_non_strong_type()
    {
        Assert.Throws<ArgumentException>(() => new StrongTypeConverter(typeof(TPrimitive)));
    }

    [Fact]
    public void Strong_type_uses_the_converter()
    {
        Assert.IsType<StrongTypeConverter<TStrongType, TPrimitive>>(_converter);
    }

    [Fact]
    public void Can_convert_from_primitive()
    {
        Assert.True(_converter.CanConvertFrom(typeof(TPrimitive)));
    }

    [Fact]
    public void Can_convert_from_string()
    {
        Assert.True(_converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void Can_convert_to_primitive()
    {
        Assert.True(_converter.CanConvertTo(typeof(TPrimitive)));
    }

    [Fact]
    public void Can_convert_to_string()
    {
        Assert.True(_converter.CanConvertTo(typeof(string)));
    }

    [Theory]
    [MemberData(nameof(PrimitivesData))]
    public void Strong_type_converts_from_its_primitive(TPrimitive primitive)
    {
        var strongType = (TStrongType)_converter.ConvertFrom(primitive)!;

        Assert.Equal(primitive, strongType.AsPrimitive());
    }

    [Theory]
    [MemberData(nameof(StrongTypesData))]
    public void Strong_type_converts_to_its_primitive(TStrongType strongType)
    {
        var primitive = (TPrimitive)_converter.ConvertTo(strongType, typeof(TPrimitive))!;

        Assert.Equal(strongType.AsPrimitive(), primitive);
    }

    [Theory]
    [MemberData(nameof(StrongTypesData))]
    public void Strong_type_converts_to_string_like_its_primitive(TStrongType strongType)
    {
        var primitiveText = _primitiveConverter.ConvertTo(
            null,
            CultureInfo.InvariantCulture,
            strongType.AsPrimitive(),
            typeof(string));

        var strongTypeText = _converter.ConvertTo(
            null, 
            CultureInfo.InvariantCulture,
            strongType,
            typeof(string));

        Assert.Equal(primitiveText, strongTypeText);
    }

    [Theory]
    [MemberData(nameof(StringsData))]
    public void Strong_type_converts_from_string_like_its_primitive(string text)
    {
        var primitive = _primitiveConverter.ConvertFrom(null, CultureInfo.InvariantCulture, text);
        var strongType = (TStrongType)_converter.ConvertFrom(null, CultureInfo.InvariantCulture, text)!;

        Assert.Equal(primitive, strongType.AsPrimitive());
    }
}
