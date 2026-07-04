#pragma warning disable xUnit1019

using System.ComponentModel;
using System.Globalization;
using SuperStrong.Types.Converters;

namespace SuperStrong.Types.Tests.Converters;

public abstract class StrongTypeConverterTests<TStrongType, TPrimitive, TPrimitiveSamples, TInvalidPrimitiveSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
    where TInvalidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
{
    private static readonly TypeConverter _converter = TypeDescriptor.GetConverter(typeof(TStrongType));
    private static readonly TypeConverter _primitiveConverter = TypeDescriptor.GetConverter(typeof(TPrimitive));

    public static TheoryData<TPrimitive> PrimitiveSamples { get; } = new TPrimitiveSamples();
    public static TheoryData<TPrimitive> InvalidPrimitiveSamples { get; } = new TInvalidPrimitiveSamples();
    public static TheoryData<TStrongType> StrongTypeSamples { get; } = CreateStrongTypeSamples();
    public static TheoryData<string> StringSamples { get; } = CreateStringSamples();

    private static TheoryData<TStrongType> CreateStrongTypeSamples()
    {
        return new(PrimitiveSamples.Select(primitive => TStrongType.From(primitive)));
    }

    private static TheoryData<string> CreateStringSamples()
    {
        return new(
            PrimitiveSamples.Select(primitive =>
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
    [MemberData(nameof(PrimitiveSamples))]
    public void Strong_type_converts_from_its_primitive(TPrimitive primitive)
    {
        var strongType = (TStrongType)_converter.ConvertFrom(primitive)!;

        Assert.Equal(primitive, strongType.AsPrimitive());
    }

    [Theory(SkipTestWithoutData = true)]
    [MemberData(nameof(InvalidPrimitiveSamples))]
    public void Strong_type_does_not_convert_from_an_invalid_primitive(TPrimitive primitive)
    {
        var exception = Assert.Throws<StrongTypeValidationException>(() => _converter.ConvertFrom(primitive));

        Assert.Equal(typeof(TStrongType), exception.StrongType);
        Assert.Equal(primitive, exception.Value);
    }

    [Theory]
    [MemberData(nameof(StrongTypeSamples))]
    public void Strong_type_converts_to_its_primitive(TStrongType strongType)
    {
        var primitive = (TPrimitive)_converter.ConvertTo(strongType, typeof(TPrimitive))!;

        Assert.Equal(strongType.AsPrimitive(), primitive);
    }

    [Theory]
    [MemberData(nameof(StrongTypeSamples))]
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
    [MemberData(nameof(StringSamples))]
    public void Strong_type_converts_from_string_like_its_primitive(string text)
    {
        var primitive = _primitiveConverter.ConvertFrom(null, CultureInfo.InvariantCulture, text);
        var strongType = (TStrongType)_converter.ConvertFrom(null, CultureInfo.InvariantCulture, text)!;

        Assert.Equal(primitive, strongType.AsPrimitive());
    }
}
