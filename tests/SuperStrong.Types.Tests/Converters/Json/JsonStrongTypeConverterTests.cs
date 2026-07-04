#pragma warning disable xUnit1019

using System.Text.Json;
using SuperStrong.Types.Converters;

namespace SuperStrong.Types.Tests.Converters;

public abstract class JsonStrongTypeConverterTests<TStrongType, TPrimitive, TPrimitiveSamples, TInvalidPrimitiveSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
    where TInvalidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
{
    private static readonly JsonStrongTypeConverter<TStrongType, TPrimitive> _converter = new();
    private static readonly JsonStrongTypeConverter _converterFactory = new();

    private static readonly JsonSerializerOptions _options = new()
    {
        Converters = { _converter }
    };

    public static TheoryData<TPrimitive> PrimitiveSamples { get; } = new TPrimitiveSamples();
    public static TheoryData<TPrimitive> InvalidPrimitiveSamples { get; } = new TInvalidPrimitiveSamples();
    public static TheoryData<TStrongType> StrongTypeSamples { get; } = CreateStrongTypeSamples();
    public static TheoryData<string> JsonSamples { get; } = CreateJsonSamples();
    public static TheoryData<string> DictionaryJsonSamples { get; } = CreateDictionaryJsonSamples();

    private static TheoryData<TStrongType> CreateStrongTypeSamples()
    {
        return new(PrimitiveSamples.Select(primitive => TStrongType.From(primitive)));
    }

    private static TheoryData<string> CreateJsonSamples()
    {
        return new(PrimitiveSamples.Select(primitive => JsonSerializer.Serialize((TPrimitive)primitive, _options)));
    }

    private static TheoryData<string> CreateDictionaryJsonSamples()
    {
        return new(
            PrimitiveSamples.Select(primitive =>
                JsonSerializer.Serialize(
                    new Dictionary<TPrimitive, object>
                    {
                        [(TPrimitive)primitive] = "value"
                    },
                    _options)));
    }

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
    [MemberData(nameof(StrongTypeSamples))]
    public void Strong_type_serializes_like_its_primitive(TStrongType strongType)
    {
        var primitiveJson = JsonSerializer.Serialize(strongType.AsPrimitive(), _options);
        var strongTypeJson = JsonSerializer.Serialize(strongType, _options);

        Assert.Equal(primitiveJson, strongTypeJson);
    }

    [Theory]
    [MemberData(nameof(JsonSamples))]
    public void Strong_type_deserializes_from_its_serialized_primitive(string json)
    {
        var primitive = JsonSerializer.Deserialize<TPrimitive>(json, _options);
        var strongType = JsonSerializer.Deserialize<TStrongType>(json, _options)!;

        Assert.Equal(primitive, strongType.AsPrimitive());
    }

    [Theory]
    [MemberData(nameof(StrongTypeSamples))]
    public void Strong_type_serializes_as_dictionary_key_like_its_primitive(TStrongType strongType)
    {
        var primitiveKeysDictionary = new Dictionary<TPrimitive, object>()
        {
            [strongType.AsPrimitive()] = "value"
        };

        var strongTypeKeysDictionary = new Dictionary<TStrongType, object>()
        {
            [strongType] = "value"
        };

        var primitiveKeysDictionaryJson = JsonSerializer.Serialize(primitiveKeysDictionary, _options);
        var strongTypeKeysDictionaryJson = JsonSerializer.Serialize(strongTypeKeysDictionary, _options);

        Assert.Equal(primitiveKeysDictionaryJson, strongTypeKeysDictionaryJson);
    }

    [Theory]
    [MemberData(nameof(DictionaryJsonSamples))]
    public void Strong_type_deserializes_from_dictionary_key_like_its_primitive(string json)
    {
        var primitiveKeysDictionary = JsonSerializer.Deserialize<Dictionary<TPrimitive, string>>(json, _options)!;
        var strongTypeKeysDictionary = JsonSerializer.Deserialize<Dictionary<TStrongType, string>>(json, _options)!;

        var primitiveKeysDictionaryEntry = Assert.Single(primitiveKeysDictionary);
        var strongTypeKeysDictionaryEntry = Assert.Single(strongTypeKeysDictionary);

        Assert.Equal(primitiveKeysDictionaryEntry.Key, strongTypeKeysDictionaryEntry.Key.AsPrimitive());
        Assert.Equal(primitiveKeysDictionaryEntry.Value, strongTypeKeysDictionaryEntry.Value);
    }

    [Theory(SkipTestWithoutData = true)]
    [MemberData(nameof(InvalidPrimitiveSamples))]
    public void Strong_type_does_not_deserialize_from_an_invalid_primitive(TPrimitive primitive)
    {
        var json = JsonSerializer.Serialize(primitive, _options);

        var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TStrongType>(json, _options));

        var validationException = Assert.IsType<StrongTypeValidationException>(exception.InnerException);
        Assert.Equal(typeof(TStrongType), validationException.StrongType);
        Assert.Equal(primitive, validationException.Value);
        Assert.Equal(validationException.Message, exception.Message);
    }

    [Theory(SkipTestWithoutData = true)]
    [MemberData(nameof(InvalidPrimitiveSamples))]
    public void Strong_type_does_not_deserialize_from_an_invalid_dictionary_key(TPrimitive primitive)
    {
        var dictionary = new Dictionary<TPrimitive, object>
        {
            [primitive] = "value"
        };

        var json = JsonSerializer.Serialize(dictionary, _options);

        var exception = Assert.Throws<JsonException>(
            () => JsonSerializer.Deserialize<Dictionary<TStrongType, object>>(json, _options));

        var validationException = Assert.IsType<StrongTypeValidationException>(exception.InnerException);
        Assert.Equal(typeof(TStrongType), validationException.StrongType);
        Assert.Equal(primitive, validationException.Value);
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
