#pragma warning disable xUnit1019

using System.Text.Json;
using SuperStrong.Types.Converters;

namespace SuperStrong.Types.Tests.Converters;

public abstract class JsonStrongTypeConverterTests<TStrongType, TPrimitive, TTheoryData>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TTheoryData : notnull, TheoryData<TPrimitive>, new()
{
    private static readonly JsonStrongTypeConverter<TStrongType, TPrimitive> _converter = new();
    private static readonly JsonStrongTypeConverter _converterFactory = new();

    private static readonly JsonSerializerOptions _options = new()
    {
        Converters = { _converter }
    };

    public static TTheoryData PrimitivesData { get; } = new();

    [Fact]
    public void Converter_factory_can_convert_strong_type()
    {
        Assert.True(_converterFactory.CanConvert(typeof(TStrongType)));
    }

    [Fact]
    public void Converter_factory_can_not_convert_primitive()
    {
        Assert.False(_converterFactory.CanConvert(typeof(TPrimitive)));
    }

    [Fact]
    public void Converter_factory_creates_typed_converter()
    {
        var converter = _converterFactory.CreateConverter(typeof(TStrongType), _options);

        Assert.IsType<JsonStrongTypeConverter<TStrongType, TPrimitive>>(converter);
    }

    [Theory]
    [MemberData(nameof(PrimitivesData))]
    public void Strong_type_serializes_like_its_primitive(TPrimitive primitive)
    {
        var strongType = TStrongType.From(primitive);

        var primitiveJson = JsonSerializer.Serialize(primitive, _options);
        var strongTypeJson = JsonSerializer.Serialize(strongType, _options);

        Assert.Equal(primitiveJson, strongTypeJson);
    }

    [Theory]
    [MemberData(nameof(PrimitivesData))]
    public void Strong_type_deserializes_from_its_serialized_primitive(TPrimitive primitive)
    {
        var primitiveJson = JsonSerializer.Serialize(primitive, _options);

        var strongType = JsonSerializer.Deserialize<TStrongType>(primitiveJson, _options)!;

        Assert.Equal(primitive, strongType.AsPrimitive());
    }

    [Theory]
    [MemberData(nameof(PrimitivesData))]
    public void Strong_type_serializes_as_dictionary_key_like_its_primitive(TPrimitive primitive)
    {
        var primitiveKeysDictionary = new Dictionary<TPrimitive, object>()
        {
            [primitive] = "value"
        };

        var strongTypeKeysDictionary = new Dictionary<TStrongType, object>()
        {
            [TStrongType.From(primitive)] = "value"
        };

        var primitiveKeysDictionaryJson = JsonSerializer.Serialize(primitiveKeysDictionary, _options);
        var strongTypeKeysDictionaryJson = JsonSerializer.Serialize(strongTypeKeysDictionary, _options);

        Assert.Equal(primitiveKeysDictionaryJson, strongTypeKeysDictionaryJson);
    }

    [Theory]
    [MemberData(nameof(PrimitivesData))]
    public void Strong_type_deserializes_from_dictionary_key_like_its_primitive(TPrimitive primitive)
    {
        var primitiveKeysDictionary = new Dictionary<TPrimitive, object>()
        {
            [primitive] = "value"
        };

        var primitiveKeysDictionaryJson = JsonSerializer.Serialize(primitiveKeysDictionary, _options);

        var strongTypeKeysDictionary = JsonSerializer.Deserialize<Dictionary<TStrongType, string>>(
            primitiveKeysDictionaryJson,
            _options)!;

        var strongTypeKeysDictionaryEntry = Assert.Single(strongTypeKeysDictionary);

        Assert.Equal(primitive, strongTypeKeysDictionaryEntry.Key.AsPrimitive());
        Assert.Equal("value", strongTypeKeysDictionaryEntry.Value);
    }

    [Fact]
    public void Null_strong_type_serializes_to_null()
    {
        Assert.Equal("null", JsonSerializer.Serialize((TStrongType?)null, _options));
    }

    [Fact]
    public void Null_strong_type_deserializes_from_null()
    {
        Assert.Null(JsonSerializer.Deserialize<TStrongType>("null", _options));
    }
}
