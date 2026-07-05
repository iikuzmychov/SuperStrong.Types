using System.Text.Json;
using SuperStrong.Types.Converters;

namespace SuperStrong.Types.Tests.Converters;

public abstract class JsonStrongTypeConverterTests<TStrongType, TPrimitive, TValidPrimitiveSamples, TInvalidPrimitiveSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TValidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
    where TInvalidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
{
    private static readonly JsonStrongTypeConverter<TStrongType, TPrimitive> _converter = new();
    private static readonly JsonStrongTypeConverter _converterFactory = new();

    private static readonly JsonSerializerOptions _options = new()
    {
        Converters = { _converter }
    };

    public static TheoryData<TPrimitive> ValidPrimitiveSamples { get; } = new TValidPrimitiveSamples();
    public static TheoryData<TPrimitive> InvalidPrimitiveSamples { get; } = new TInvalidPrimitiveSamples();
    public static TheoryData<TStrongType> StrongTypeSamples { get; } = CreateStrongTypeSamples();
    public static TheoryData<string> ValidJsonSamples { get; } = CreateJsonSamples(ValidPrimitiveSamples);
    public static TheoryData<string> InvalidJsonSamples { get; } = CreateJsonSamples(InvalidPrimitiveSamples);
    public static TheoryData<string> ValidDictionaryJsonSamples { get; } = CreateDictionaryJsonSamples(ValidPrimitiveSamples);
    public static TheoryData<string> InvalidDictionaryJsonSamples { get; } = CreateDictionaryJsonSamples(InvalidPrimitiveSamples);

    private static TheoryData<TStrongType> CreateStrongTypeSamples()
    {
        return new(ValidPrimitiveSamples.Select(primitive => TStrongType.From(primitive)));
    }

    private static TheoryData<string> CreateJsonSamples(TheoryData<TPrimitive> primitiveSamples)
    {
        return new(primitiveSamples.Select(primitive => JsonSerializer.Serialize((TPrimitive)primitive, _options)));
    }

    private static TheoryData<string> CreateDictionaryJsonSamples(TheoryData<TPrimitive> primitiveSamples)
    {
        return new(
            primitiveSamples.Select(primitive =>
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
    [MemberData(nameof(ValidJsonSamples))]
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
    [MemberData(nameof(ValidDictionaryJsonSamples))]
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
    [MemberData(nameof(InvalidJsonSamples))]
    public void Strong_type_does_not_deserialize_from_an_invalid_primitive(string json)
    {
        var primitive = JsonSerializer.Deserialize<TPrimitive>(json, _options);

        var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TStrongType>(json, _options));

        var validationException = Assert.IsType<StrongTypeValidationException>(exception.InnerException);
        Assert.Equal(typeof(TStrongType), validationException.StrongType);
        Assert.Equal(primitive, validationException.Value);
        Assert.Equal(validationException.Message, exception.Message);
    }

    [Theory(SkipTestWithoutData = true)]
    [MemberData(nameof(InvalidDictionaryJsonSamples))]
    public void Strong_type_does_not_deserialize_from_an_invalid_dictionary_key(string json)
    {
        var primitiveKeysDictionaryEntry = Assert.Single(
            JsonSerializer.Deserialize<Dictionary<TPrimitive, string>>(json, _options)!);

        var exception = Assert.Throws<JsonException>(
            () => JsonSerializer.Deserialize<Dictionary<TStrongType, string>>(json, _options));

        var validationException = Assert.IsType<StrongTypeValidationException>(exception.InnerException);
        Assert.Equal(typeof(TStrongType), validationException.StrongType);
        Assert.Equal(primitiveKeysDictionaryEntry.Key, validationException.Value);
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

public sealed class BoolJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples, StrongBool.InvalidPrimitiveSamples>;

public sealed class ByteJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples, StrongByte.InvalidPrimitiveSamples>;

public sealed class SByteJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongSByte, sbyte, StrongSByte.ValidPrimitiveSamples, StrongSByte.InvalidPrimitiveSamples>;

public sealed class ShortJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongShort, short, StrongShort.ValidPrimitiveSamples, StrongShort.InvalidPrimitiveSamples>;

public sealed class UShortJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongUShort, ushort, StrongUShort.ValidPrimitiveSamples, StrongUShort.InvalidPrimitiveSamples>;

public sealed class IntJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongInt, int, StrongInt.ValidPrimitiveSamples, StrongInt.InvalidPrimitiveSamples>;

public sealed class UIntJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongUInt, uint, StrongUInt.ValidPrimitiveSamples, StrongUInt.InvalidPrimitiveSamples>;

public sealed class LongJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongLong, long, StrongLong.ValidPrimitiveSamples, StrongLong.InvalidPrimitiveSamples>;

public sealed class ULongJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongULong, ulong, StrongULong.ValidPrimitiveSamples, StrongULong.InvalidPrimitiveSamples>;

public sealed class FloatJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples, StrongFloat.InvalidPrimitiveSamples>;

public sealed class DoubleJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples, StrongDouble.InvalidPrimitiveSamples>;

public sealed class DecimalJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples, StrongDecimal.InvalidPrimitiveSamples>;

public sealed class StringJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongString, string, StrongString.ValidPrimitiveSamples, StrongString.InvalidPrimitiveSamples>;

public sealed class CharJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongChar, char, StrongChar.ValidPrimitiveSamples, StrongChar.InvalidPrimitiveSamples>;

public sealed class GuidJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples, StrongGuid.InvalidPrimitiveSamples>;

public sealed class DateTimeJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples, StrongDateTime.InvalidPrimitiveSamples>;

public sealed class DateTimeOffsetJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples, StrongDateTimeOffset.InvalidPrimitiveSamples>;

public sealed class DateOnlyJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples, StrongDateOnly.InvalidPrimitiveSamples>;

public sealed class TimeOnlyJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples, StrongTimeOnly.InvalidPrimitiveSamples>;

public sealed class TimeSpanJsonStrongTypeConverterTests
    : JsonStrongTypeConverterTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples, StrongTimeSpan.InvalidPrimitiveSamples>;
