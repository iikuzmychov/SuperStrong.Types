using Newtonsoft.Json;
using SuperStrong.Types.Reflection;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace SuperStrong.Types.NewtonsoftJson;

public sealed class JsonStrongTypeDictionaryConverter : JsonConverter
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> _converterCache = new();

    public override bool CanConvert(Type objectType)
    {
        return TryGetKeyInfo(objectType, out _, out _, out _);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var converter = GetConverter(objectType);
        
        return converter.ReadJson(reader, objectType, existingValue, serializer);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        var converter = GetConverter(value.GetType());
        converter.WriteJson(writer, value, serializer);
    }

    private static bool TryGetKeyInfo(
        Type dictionaryType,
        [NotNullWhen(true)] out Type? keyType,
        [NotNullWhen(true)] out Type? primitiveKeyType,
        [NotNullWhen(true)] out Type? valueType)
    {
        foreach (var dictionaryInterface in GetDictionaryInterfaces(dictionaryType))
        {
            var arguments = dictionaryInterface.GetGenericArguments();

            if (arguments[0].GetStrongTypeInfo() is { } keyInfo)
            {
                keyType = keyInfo.ClrType;
                primitiveKeyType = keyInfo.PrimitiveType;
                valueType = arguments[1];

                return true;
            }
        }

        keyType = null;
        primitiveKeyType = null;
        valueType = null;

        return false;
    }

    private static IEnumerable<Type> GetDictionaryInterfaces(Type type)
    {
        var candidates = type.IsInterface
            ? new[] { type }.Concat(type.GetInterfaces())
            : type.GetInterfaces();

        foreach (var candidate in candidates)
        {
            if (!candidate.IsGenericType)
            {
                continue;
            }

            var definition = candidate.GetGenericTypeDefinition();

            if (definition == typeof(IDictionary<,>) || definition == typeof(IReadOnlyDictionary<,>))
            {
                yield return candidate;
            }
        }
    }

    private static JsonConverter GetConverter(Type dictionaryType)
    {
        return _converterCache.GetOrAdd(dictionaryType, static type =>
        {
            TryGetKeyInfo(type, out var keyType, out var primitiveKeyType, out var valueType);

            var converterType = typeof(JsonStrongTypeDictionaryConverter<,,>)
                .MakeGenericType(keyType!, primitiveKeyType!, valueType!);

            return (JsonConverter)Activator.CreateInstance(converterType)!;
        });
    }
}

public sealed class JsonStrongTypeDictionaryConverter<TKey, TPrimitiveKey, TValue> : JsonConverter
    where TKey : IStrongType<TKey, TPrimitiveKey>
    where TPrimitiveKey : notnull
{
    public override bool CanConvert(Type objectType)
    {
        return
            typeof(IDictionary<TKey, TValue>).IsAssignableFrom(objectType) ||
            typeof(IReadOnlyDictionary<TKey, TValue>).IsAssignableFrom(objectType);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType is JsonToken.Null)
        {
            return null;
        }

        var primitiveDictionary = serializer.Deserialize<Dictionary<TPrimitiveKey, TValue>>(reader)!;
        var dictionary = new Dictionary<TKey, TValue>(primitiveDictionary.Count);

        foreach (var (primitiveKey, value) in primitiveDictionary)
        {
            dictionary[CreateKey(primitiveKey)] = value;
        }

        return Materialize(objectType, dictionary);
    }

    private static TKey CreateKey(TPrimitiveKey primitive)
    {
        try
        {
            return TKey.From(primitive);
        }
        catch (StrongTypeValidationException exception)
        {
            throw new JsonSerializationException(exception.Message, exception);
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        var primitiveDictionary = new Dictionary<TPrimitiveKey, TValue>();

        foreach (var (strongKey, item) in (IEnumerable<KeyValuePair<TKey, TValue>>)value)
        {
            primitiveDictionary[strongKey.AsPrimitive()] = item;
        }

        serializer.Serialize(writer, primitiveDictionary);
    }

    private static object Materialize(Type objectType, Dictionary<TKey, TValue> dictionary)
    {
        if (objectType.IsAssignableFrom(typeof(Dictionary<TKey, TValue>)))
        {
            return dictionary;
        }

        if (objectType == typeof(ImmutableDictionary<TKey, TValue>) ||
            objectType == typeof(IImmutableDictionary<TKey, TValue>))
        {
            return ImmutableDictionary.CreateRange(dictionary);
        }

        if (objectType == typeof(ImmutableSortedDictionary<TKey, TValue>))
        {
            return ImmutableSortedDictionary.CreateRange(dictionary);
        }

        if (objectType == typeof(FrozenDictionary<TKey, TValue>))
        {
            return dictionary.ToFrozenDictionary();
        }

        if (typeof(IDictionary<TKey, TValue>).IsAssignableFrom(objectType) &&
            objectType.GetConstructor(Type.EmptyTypes) is not null)
        {
            var result = (IDictionary<TKey, TValue>)Activator.CreateInstance(objectType)!;

            foreach (var pair in dictionary)
            {
                result.Add(pair);
            }

            return result;
        }

        var parameterizedConstructor =
            objectType.GetConstructor([typeof(IDictionary<TKey, TValue>)])
            ?? objectType.GetConstructor([typeof(IEnumerable<KeyValuePair<TKey, TValue>>)]);

        if (parameterizedConstructor is not null)
        {
            return parameterizedConstructor.Invoke([dictionary]);
        }

        throw new JsonSerializationException($"Cannot create an instance of dictionary type '{objectType}'.");
    }
}
