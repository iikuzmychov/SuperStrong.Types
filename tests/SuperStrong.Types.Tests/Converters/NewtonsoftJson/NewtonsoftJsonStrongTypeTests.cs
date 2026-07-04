using Newtonsoft.Json;
using SuperStrong.Types.NewtonsoftJson;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace SuperStrong.Types.Tests.Converters;

public abstract class NewtonsoftJsonStrongTypeTests<TStrongType, TPrimitive, TPrimitiveSamples, TInvalidPrimitiveSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
    where TInvalidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
{
    private static readonly JsonSerializerSettings _settings = CreateSettings();

    public static TheoryData<TPrimitive> PrimitiveSamples { get; } = new TPrimitiveSamples();
    public static TheoryData<TPrimitive> InvalidPrimitiveSamples { get; } = new TInvalidPrimitiveSamples();
    public static TheoryData<TStrongType> StrongTypeSamples { get; } = CreateStrongTypeSamples();
    public static TheoryData<string> JsonSamples { get; } = CreateJsonSamples();
    public static TheoryData<string> DictionaryValueJsonSamples { get; } = CreateDictionaryValueJsonSamples();
    public static TheoryData<Type> StrongTypeKeyDictionaryTypeSamples { get; } = CreateDictionaryTypeSamples(typeof(TStrongType), typeof(object));
    public static TheoryData<Type> PrimitiveKeyDictionaryTypeSamples { get; } = CreateDictionaryTypeSamples(typeof(TPrimitive), typeof(object));
    public static TheoryData<Type> StrongTypeValueDictionaryTypeSamples { get; } = CreateDictionaryTypeSamples(typeof(object), typeof(TStrongType));
    public static TheoryData<Type, Type, string> DictionaryDeserializationSamples { get; } = CreateDictionaryDeserializationSamples();
    public static TheoryData<Type, Type, TStrongType> StrongTypeKeyDictionarySerializationSamples { get; } = CreateDictionarySerializationSamples(typeof(TPrimitive), typeof(object), typeof(TStrongType), typeof(object));
    public static TheoryData<Type, Type, TStrongType> StrongTypeValueDictionarySerializationSamples { get; } = CreateDictionarySerializationSamples(typeof(object), typeof(TPrimitive), typeof(object), typeof(TStrongType));

    private static JsonSerializerSettings CreateSettings()
    {
        return new JsonSerializerSettings
        {
            FloatParseHandling = typeof(TPrimitive) == typeof(decimal)
                ? FloatParseHandling.Decimal
                : default,
            Converters =
            {
                new JsonStrongTypeConverter(),
                new JsonStrongTypeDictionaryConverter()
            },
        };
    }

    private static TheoryData<TStrongType> CreateStrongTypeSamples()
    {
        return new(PrimitiveSamples.Select(primitive => TStrongType.From(primitive)));
    }

    private static TheoryData<string> CreateJsonSamples()
    {
        return new(PrimitiveSamples.Select(primitive => JsonConvert.SerializeObject((TPrimitive)primitive)));
    }

    private static TheoryData<string> CreateDictionaryValueJsonSamples()
    {
        return new(
            PrimitiveSamples.Select(primitive =>
                JsonConvert.SerializeObject(
                    new Dictionary<string, TPrimitive> { ["key"] = (TPrimitive)primitive })));
    }

    private static Type[] CreateDictionaryTypeDefinitions()
    {
        return
        [
            typeof(Dictionary<,>),
            typeof(IDictionary<,>),
            typeof(SortedDictionary<,>),
            typeof(ConcurrentDictionary<,>),
            typeof(ReadOnlyDictionary<,>),
            typeof(IReadOnlyDictionary<,>),
            typeof(ImmutableDictionary<,>),
            typeof(ImmutableSortedDictionary<,>),
            typeof(IImmutableDictionary<,>),
            typeof(FrozenDictionary<,>),
        ];
    }

    private static TheoryData<Type> CreateDictionaryTypeSamples(Type keyType, Type valueType)
    {
        return new(CreateDictionaryTypeDefinitions().Select(definition => definition.MakeGenericType(keyType, valueType)));
    }

    private static TheoryData<Type, Type, string> CreateDictionaryDeserializationSamples()
    {
        var data = new TheoryData<Type, Type, string>();

        // Newtonsoft cannot materialize a primitive-keyed FrozenDictionary, so the primitive
        // reference stays a plain Dictionary; it only supplies the expected entry.
        var primitiveDictionaryType = typeof(Dictionary<TPrimitive, object>);

        var jsons = PrimitiveSamples.Select(primitive =>
            JsonConvert.SerializeObject(
                new Dictionary<TPrimitive, object> { [(TPrimitive)primitive] = "value" }));

        foreach (var json in jsons)
        {
            foreach (var definition in CreateDictionaryTypeDefinitions())
            {
                var strongDictionaryType = definition.MakeGenericType(typeof(TStrongType), typeof(object));

                data.Add(primitiveDictionaryType, strongDictionaryType, json);
            }
        }

        return data;
    }

    private static TheoryData<Type, Type, TStrongType> CreateDictionarySerializationSamples(
        Type primitiveKeyType,
        Type primitiveValueType,
        Type strongTypeKeyType,
        Type strongTypeValueType)
    {
        var data = new TheoryData<Type, Type, TStrongType>();

        foreach (var definition in CreateDictionaryTypeDefinitions())
        {
            var primitiveDictionaryType = definition.MakeGenericType(primitiveKeyType, primitiveValueType);
            var strongDictionaryType = definition.MakeGenericType(strongTypeKeyType, strongTypeValueType);

            foreach (var strongTypeRow in StrongTypeSamples)
            {
                data.Add(primitiveDictionaryType, strongDictionaryType, strongTypeRow.Data);
            }
        }

        return data;
    }

    private static object CreateSingleEntryDictionary<TKey, TValue>(Type dictionaryType, TKey key, TValue value)
        where TKey : notnull
    {
        var dictionary = new Dictionary<TKey, TValue>
        {
            [key] = value
        };

        var definition = dictionaryType.GetGenericTypeDefinition();

        if (definition == typeof(ImmutableDictionary<,>) || definition == typeof(IImmutableDictionary<,>))
        {
            return dictionary.ToImmutableDictionary();
        }

        if (definition == typeof(ImmutableSortedDictionary<,>))
        {
            return dictionary.ToImmutableSortedDictionary();
        }

        if (definition == typeof(FrozenDictionary<,>))
        {
            return dictionary.ToFrozenDictionary();
        }

        if (definition == typeof(SortedDictionary<,>))
        {
            return new SortedDictionary<TKey, TValue>(dictionary);
        }

        if (definition == typeof(ConcurrentDictionary<,>))
        {
            return new ConcurrentDictionary<TKey, TValue>(dictionary);
        }

        if (definition == typeof(ReadOnlyDictionary<,>))
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        return dictionary;
    }

    [Fact]
    public void Value_converter_can_convert_strong_type()
    {
        var converter = new JsonStrongTypeConverter();

        Assert.True(converter.CanConvert(typeof(TStrongType)));
    }

    [Fact]
    public void Value_converter_can_not_convert_primitive()
    {
        var converter = new JsonStrongTypeConverter();

        Assert.False(converter.CanConvert(typeof(TPrimitive)));
    }

    [Theory]
    [MemberData(nameof(StrongTypeKeyDictionaryTypeSamples))]
    public void Dictionary_converter_can_convert_dictionary_with_strong_type_key(Type dictionaryType)
    {
        var converter = new JsonStrongTypeDictionaryConverter();

        Assert.True(converter.CanConvert(dictionaryType));
    }

    [Theory]
    [MemberData(nameof(PrimitiveKeyDictionaryTypeSamples))]
    public void Dictionary_converter_can_not_convert_dictionary_with_primitive_key(Type dictionaryType)
    {
        var converter = new JsonStrongTypeDictionaryConverter();

        Assert.False(converter.CanConvert(dictionaryType));
    }

    [Theory]
    [MemberData(nameof(StrongTypeValueDictionaryTypeSamples))]
    public void Dictionary_converter_can_not_convert_dictionary_with_strong_type_value(Type dictionaryType)
    {
        var converter = new JsonStrongTypeDictionaryConverter();

        Assert.False(converter.CanConvert(dictionaryType));
    }

    [Fact]
    public void Dictionary_converter_can_not_convert_non_dictionary()
    {
        var converter = new JsonStrongTypeDictionaryConverter();

        Assert.False(converter.CanConvert(typeof(TStrongType)));
        Assert.False(converter.CanConvert(typeof(TPrimitive)));
    }


    [Fact]
    public void Null_strong_type_serializes_to_null()
    {
        Assert.Equal("null", JsonConvert.SerializeObject((TStrongType?)null, _settings));
    }

    [Fact]
    public void Null_strong_type_deserializes_from_null()
    {
        Assert.Null(JsonConvert.DeserializeObject<TStrongType>("null", _settings));
    }

    [Theory]
    [MemberData(nameof(StrongTypeSamples))]
    public void Strong_type_serializes_like_its_primitive(TStrongType strongType)
    {
        var primitiveJson = JsonConvert.SerializeObject(strongType.AsPrimitive(), _settings);
        var strongTypeJson = JsonConvert.SerializeObject(strongType, _settings);

        Assert.Equal(primitiveJson, strongTypeJson);
    }

    [Theory]
    [MemberData(nameof(JsonSamples))]
    public void Strong_type_deserializes_like_its_primitive(string json)
    {
        var primitive = JsonConvert.DeserializeObject<TPrimitive>(json, _settings);
        var strongType = JsonConvert.DeserializeObject<TStrongType>(json, _settings)!;

        Assert.Equal(primitive, strongType.AsPrimitive());
    }

    [Theory(SkipTestWithoutData = true)]
    [MemberData(nameof(InvalidPrimitiveSamples))]
    public void Strong_type_does_not_deserialize_from_an_invalid_primitive(TPrimitive primitive)
    {
        var json = JsonConvert.SerializeObject(primitive);

        var exception = Assert.Throws<JsonSerializationException>(
            () => JsonConvert.DeserializeObject<TStrongType>(json, _settings));

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

        var json = JsonConvert.SerializeObject(dictionary);

        var exception = Assert.Throws<JsonSerializationException>(
            () => JsonConvert.DeserializeObject<Dictionary<TStrongType, object>>(json, _settings));

        var validationException = Assert.IsType<StrongTypeValidationException>(exception.InnerException);
        Assert.Equal(typeof(TStrongType), validationException.StrongType);
        Assert.Equal(primitive, validationException.Value);
    }

    [Theory]
    [MemberData(nameof(StrongTypeKeyDictionarySerializationSamples))]
    public void Strong_type_serializes_as_dictionary_key_like_its_primitive(
        Type primitiveKeysDictionaryType,
        Type strongTypekeysDictionaryType,
        TStrongType strongType)
    {
        var primitiveKeysDictionary = CreateSingleEntryDictionary<TPrimitive, object>(
            primitiveKeysDictionaryType, strongType.AsPrimitive(), "value");

        var strongTypeKeysDictionary = CreateSingleEntryDictionary<TStrongType, object>(
            strongTypekeysDictionaryType, strongType, "value");

        var primitiveKeysDictionaryJson = JsonConvert.SerializeObject(primitiveKeysDictionary, _settings);
        var strongTypeKeysDictionaryJson = JsonConvert.SerializeObject(strongTypeKeysDictionary, _settings);

        Assert.Equal(primitiveKeysDictionaryJson, strongTypeKeysDictionaryJson);
    }

    [Theory]
    [MemberData(nameof(DictionaryDeserializationSamples))]
    public void Strong_type_deserializes_as_dictionary_key_like_its_primitive(
        Type primitiveKeysDictionaryType,
        Type strongTypeKeysDictionaryType,
        string json)
    {
        var primitiveKeysDictionary = (IEnumerable<KeyValuePair<TPrimitive, object>>)JsonConvert.DeserializeObject(
            json,
            primitiveKeysDictionaryType,
            _settings)!;

        var strongTypeKeyDdictionary = (IEnumerable<KeyValuePair<TStrongType, object>>)JsonConvert.DeserializeObject(
            json,
            strongTypeKeysDictionaryType,
            _settings)!;

        Assert.IsAssignableFrom(primitiveKeysDictionaryType, primitiveKeysDictionary);
        Assert.IsAssignableFrom(strongTypeKeysDictionaryType, strongTypeKeyDdictionary);

        var primitiveKeysDictionaryEntry = Assert.Single(primitiveKeysDictionary);
        var strongTypeKeysDictionaryEntry = Assert.Single(strongTypeKeyDdictionary);

        Assert.Equal(primitiveKeysDictionaryEntry.Key, strongTypeKeysDictionaryEntry.Key.AsPrimitive());
        Assert.Equal(primitiveKeysDictionaryEntry.Value, strongTypeKeysDictionaryEntry.Value);
    }

    [Theory]
    [MemberData(nameof(StrongTypeValueDictionarySerializationSamples))]
    public void Strong_type_serializes_as_dictionary_value_like_its_primitive(
        Type primitiveValuesDictionaryType,
        Type strongTypeValuesDictionaryType,
        TStrongType strongType)
    {
        var primitiveValuesDictionary = CreateSingleEntryDictionary<object, TPrimitive>(
            primitiveValuesDictionaryType, "key", strongType.AsPrimitive());

        var strongTypeValuesDictionary = CreateSingleEntryDictionary<object, TStrongType>(
            strongTypeValuesDictionaryType, "key", strongType);

        var primitiveValuesDictionaryJson = JsonConvert.SerializeObject(primitiveValuesDictionary, _settings);
        var strongTypeValuesDictionaryJson = JsonConvert.SerializeObject(strongTypeValuesDictionary, _settings);

        Assert.Equal(primitiveValuesDictionaryJson, strongTypeValuesDictionaryJson);
    }

    [Theory]
    [MemberData(nameof(DictionaryValueJsonSamples))]
    public void Strong_type_deserializes_as_dictionary_value_like_its_primitive(string json)
    {
        var primitiveValuesDictionaryEntry = Assert.Single(
            JsonConvert.DeserializeObject<Dictionary<string, TPrimitive>>(json, _settings)!);

        var strongTypeValuesDictionaryEntry = Assert.Single(
            JsonConvert.DeserializeObject<Dictionary<string, TStrongType>>(json, _settings)!);

        Assert.Equal(primitiveValuesDictionaryEntry.Value, strongTypeValuesDictionaryEntry.Value.AsPrimitive());
    }
}
