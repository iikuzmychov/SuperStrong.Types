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
        [NotNullWhen(true)] out Type? strongTypeKeyType,
        [NotNullWhen(true)] out Type? primitiveKeyType,
        [NotNullWhen(true)] out Type? valueType)
    {
        foreach (var dictionaryInterface in GetDictionaryInterfaces(dictionaryType))
        {
            var arguments = dictionaryInterface.GetGenericArguments();

            if (arguments[0].GetStrongTypeInfo() is { } keyInfo)
            {
                strongTypeKeyType = keyInfo.StrongType;
                primitiveKeyType = keyInfo.PrimitiveType;
                valueType = arguments[1];

                return true;
            }
        }

        strongTypeKeyType = null;
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
            TryGetKeyInfo(type, out var strongTypeKeyType, out var primitiveKeyType, out var valueType);

            var converterType = typeof(JsonStrongTypeDictionaryConverter<,,>)
                .MakeGenericType(strongTypeKeyType!, primitiveKeyType!, valueType!);

            return (JsonConverter)Activator.CreateInstance(converterType)!;
        });
    }
}

public sealed class JsonStrongTypeDictionaryConverter<TStrongTypeKey, TPrimitiveKey, TValue> : JsonConverter
    where TStrongTypeKey : IStrongType<TStrongTypeKey, TPrimitiveKey>
    where TPrimitiveKey : notnull
{
    public override bool CanConvert(Type objectType)
    {
        return
            typeof(IDictionary<TStrongTypeKey, TValue>).IsAssignableFrom(objectType) ||
            typeof(IReadOnlyDictionary<TStrongTypeKey, TValue>).IsAssignableFrom(objectType);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType is JsonToken.Null)
        {
            return null;
        }

        var primitiveDictionary = serializer.Deserialize<Dictionary<TPrimitiveKey, TValue>>(reader)!;
        var dictionary = new Dictionary<TStrongTypeKey, TValue>(primitiveDictionary.Count);

        foreach (var (primitiveKey, value) in primitiveDictionary)
        {
            dictionary[CreateKey(primitiveKey)] = value;
        }

        return Materialize(objectType, dictionary);
    }

    private static TStrongTypeKey CreateKey(TPrimitiveKey primitive)
    {
        try
        {
            return TStrongTypeKey.From(primitive);
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

        foreach (var (strongKey, item) in (IEnumerable<KeyValuePair<TStrongTypeKey, TValue>>)value)
        {
            primitiveDictionary[strongKey.AsPrimitive()] = item;
        }

        serializer.Serialize(writer, primitiveDictionary);
    }

    private static object Materialize(Type objectType, Dictionary<TStrongTypeKey, TValue> dictionary)
    {
        if (objectType.IsAssignableFrom(typeof(Dictionary<TStrongTypeKey, TValue>)))
        {
            return dictionary;
        }

        if (objectType == typeof(ImmutableDictionary<TStrongTypeKey, TValue>) ||
            objectType == typeof(IImmutableDictionary<TStrongTypeKey, TValue>))
        {
            return ImmutableDictionary.CreateRange(dictionary);
        }

        if (objectType == typeof(ImmutableSortedDictionary<TStrongTypeKey, TValue>))
        {
            return ImmutableSortedDictionary.CreateRange(dictionary);
        }

        if (objectType == typeof(FrozenDictionary<TStrongTypeKey, TValue>))
        {
            return dictionary.ToFrozenDictionary();
        }

        if (typeof(IDictionary<TStrongTypeKey, TValue>).IsAssignableFrom(objectType) &&
            objectType.GetConstructor(Type.EmptyTypes) is not null)
        {
            var result = (IDictionary<TStrongTypeKey, TValue>)Activator.CreateInstance(objectType)!;

            foreach (var pair in dictionary)
            {
                result.Add(pair);
            }

            return result;
        }

        var parameterizedConstructor =
            objectType.GetConstructor([typeof(IDictionary<TStrongTypeKey, TValue>)])
            ?? objectType.GetConstructor([typeof(IEnumerable<KeyValuePair<TStrongTypeKey, TValue>>)]);

        if (parameterizedConstructor is not null)
        {
            return parameterizedConstructor.Invoke([dictionary]);
        }

        throw new JsonSerializationException($"Cannot create an instance of dictionary type '{objectType}'.");
    }
}
