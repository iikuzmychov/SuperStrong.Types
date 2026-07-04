using Newtonsoft.Json;
using SuperStrong.Types.Reflection;
using System.Collections.Concurrent;

namespace SuperStrong.Types.NewtonsoftJson;

public sealed class JsonStrongTypeConverter : JsonConverter
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> _converterCache = new();

    public override bool CanConvert(Type objectType)
    {
        return objectType.GetStrongTypeInfo() is not null;
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

    private static JsonConverter GetConverter(Type strongType)
    {
        return _converterCache.GetOrAdd(strongType, static type =>
        {
            var strongTypeInfo = type.GetStrongTypeInfo()!;

            var converterType = typeof(JsonStrongTypeConverter<,>)
                .MakeGenericType(strongTypeInfo.StrongType, strongTypeInfo.PrimitiveType);

            return (JsonConverter)Activator.CreateInstance(converterType)!;
        });
    }
}

public sealed class JsonStrongTypeConverter<TStrongType, TPrimitive> : JsonConverter<TStrongType>
    where TStrongType : IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
{
    public override TStrongType? ReadJson(
        JsonReader reader,
        Type objectType,
        TStrongType? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType is JsonToken.Null)
        {
            if (default(TStrongType) is not null)
            {
                throw new JsonSerializationException($"Cannot convert null to '{typeof(TStrongType)}'.");
            }

            return default;
        }

        var primitive = serializer.Deserialize<TPrimitive>(reader);

        try
        {
            return TStrongType.From(primitive!);
        }
        catch (StrongTypeValidationException exception)
        {
            throw new JsonSerializationException(exception.Message, exception);
        }
    }

    public override void WriteJson(JsonWriter writer, TStrongType? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        serializer.Serialize(writer, value.AsPrimitive());
    }
}
