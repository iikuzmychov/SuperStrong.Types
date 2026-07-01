using Newtonsoft.Json;
using SuperStrong.Types.NewtonsoftJson;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace SuperStrong.Types.Tests.Converters;

public abstract class NewtonsoftJsonStrongTypeTests<TStrongType, TPrimitive, TPrimitivesData>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TPrimitivesData : notnull, TheoryData<TPrimitive>, new()
{
    private static readonly JsonSerializerSettings _settings = CreateSettings();

    public static TheoryData<TPrimitive> PrimitivesData { get; } = new TPrimitivesData();
    public static TheoryData<TStrongType> StrongTypesData { get; } = CreateStrongTypesData();
    public static TheoryData<string> JsonsData { get; } = CreateJsonsData();
    public static TheoryData<string> DictionaryValueJsonsData { get; } = CreateDictionaryValueJsonsData();
    public static TheoryData<Type> StrongTypeKeyDictionaryTypesData { get; } = CreateDictionaryTypesData(typeof(TStrongType), typeof(object));
    public static TheoryData<Type> PrimitiveKeyDictionaryTypesData { get; } = CreateDictionaryTypesData(typeof(TPrimitive), typeof(object));
    public static TheoryData<Type> StrongTypeValueDictionaryTypesData { get; } = CreateDictionaryTypesData(typeof(object), typeof(TStrongType));
    public static TheoryData<Type, Type, string> DictionaryDeserializationData { get; } = CreateDictionaryDeserializationData();
    public static TheoryData<Type, Type, TStrongType> StrongTypeKeyDictionarySerializationData { get; } = CreateDictionarySerializationData(typeof(TPrimitive), typeof(object), typeof(TStrongType), typeof(object));
    public static TheoryData<Type, Type, TStrongType> StrongTypeValueDictionarySerializationData { get; } = CreateDictionarySerializationData(typeof(object), typeof(TPrimitive), typeof(object), typeof(TStrongType));

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

    private static TheoryData<TStrongType> CreateStrongTypesData()
    {
        return new(PrimitivesData.Select(primitive => TStrongType.From(primitive)));
    }

    private static TheoryData<string> CreateJsonsData()
    {
        return new(PrimitivesData.Select(primitive => JsonConvert.SerializeObject((TPrimitive)primitive)));
    }

    private static TheoryData<string> CreateDictionaryValueJsonsData()
    {
        return new(
            PrimitivesData.Select(primitive =>
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

    private static TheoryData<Type> CreateDictionaryTypesData(Type keyType, Type valueType)
    {
        return new(CreateDictionaryTypeDefinitions().Select(definition => definition.MakeGenericType(keyType, valueType)));
    }

    private static TheoryData<Type, Type, string> CreateDictionaryDeserializationData()
    {
        var data = new TheoryData<Type, Type, string>();

        // Newtonsoft cannot materialize a primitive-keyed FrozenDictionary, so the primitive
        // reference stays a plain Dictionary; it only supplies the expected entry.
        var primitiveDictionaryType = typeof(Dictionary<TPrimitive, object>);

        var jsons = PrimitivesData.Select(primitive =>
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

    private static TheoryData<Type, Type, TStrongType> CreateDictionarySerializationData(
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

            foreach (var strongTypeRow in StrongTypesData)
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
    [MemberData(nameof(StrongTypeKeyDictionaryTypesData))]
    public void Dictionary_converter_can_convert_dictionary_with_strong_type_key(Type dictionaryType)
    {
        var converter = new JsonStrongTypeDictionaryConverter();

        Assert.True(converter.CanConvert(dictionaryType));
    }

    [Theory]
    [MemberData(nameof(PrimitiveKeyDictionaryTypesData))]
    public void Dictionary_converter_can_not_convert_dictionary_with_primitive_key(Type dictionaryType)
    {
        var converter = new JsonStrongTypeDictionaryConverter();

        Assert.False(converter.CanConvert(dictionaryType));
    }

    [Theory]
    [MemberData(nameof(StrongTypeValueDictionaryTypesData))]
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
    [MemberData(nameof(StrongTypesData))]
    public void Strong_type_serializes_like_its_primitive(TStrongType strongType)
    {
        var primitiveJson = JsonConvert.SerializeObject(strongType.AsPrimitive(), _settings);
        var strongTypeJson = JsonConvert.SerializeObject(strongType, _settings);

        Assert.Equal(primitiveJson, strongTypeJson);
    }

    [Theory]
    [MemberData(nameof(JsonsData))]
    public void Strong_type_deserializes_like_its_primitive(string json)
    {
        var primitive = JsonConvert.DeserializeObject<TPrimitive>(json, _settings);
        var strongType = JsonConvert.DeserializeObject<TStrongType>(json, _settings)!;

        Assert.Equal(primitive, strongType.AsPrimitive());
    }

    [Theory]
    [MemberData(nameof(StrongTypeKeyDictionarySerializationData))]
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
    [MemberData(nameof(DictionaryDeserializationData))]
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
    [MemberData(nameof(StrongTypeValueDictionarySerializationData))]
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
    [MemberData(nameof(DictionaryValueJsonsData))]
    public void Strong_type_deserializes_as_dictionary_value_like_its_primitive(string json)
    {
        var primitiveValuesDictionaryEntry = Assert.Single(
            JsonConvert.DeserializeObject<Dictionary<string, TPrimitive>>(json, _settings)!);

        var strongTypeValuesDictionaryEntry = Assert.Single(
            JsonConvert.DeserializeObject<Dictionary<string, TStrongType>>(json, _settings)!);

        Assert.Equal(primitiveValuesDictionaryEntry.Value, strongTypeValuesDictionaryEntry.Value.AsPrimitive());
    }
}
