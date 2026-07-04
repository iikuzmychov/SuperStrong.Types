using SuperStrong.Types.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SuperStrong.Types.Converters;

public sealed class JsonStrongTypeConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.GetStrongTypeInfo() is not null;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var strongTypeInfo = typeToConvert.GetStrongTypeInfo()!;

        var converterType = typeof(JsonStrongTypeConverter<,>)
            .MakeGenericType(strongTypeInfo.StrongType, strongTypeInfo.PrimitiveType);

        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

public sealed class JsonStrongTypeConverter<TStrongType, TPrimitive> : JsonConverter<TStrongType>
    where TStrongType : IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
{
    public override TStrongType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var primitive = JsonSerializer.Deserialize<TPrimitive>(ref reader, options);

        return CreateStrongType(primitive!);
    }

    public override void Write(Utf8JsonWriter writer, TStrongType value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.AsPrimitive(), options);
    }

    public override TStrongType ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var primitiveConverter = GetPrimitiveConverter(options);
        var primitive = primitiveConverter.ReadAsPropertyName(ref reader, typeof(TPrimitive), options);

        return CreateStrongType(primitive);
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, TStrongType value, JsonSerializerOptions options)
    {
        var primitiveConverter = GetPrimitiveConverter(options);
        primitiveConverter.WriteAsPropertyName(writer, value.AsPrimitive(), options);
    }

    private static JsonConverter<TPrimitive> GetPrimitiveConverter(JsonSerializerOptions options)
    {
        return (JsonConverter<TPrimitive>)options.GetConverter(typeof(TPrimitive));
    }

    private static TStrongType CreateStrongType(TPrimitive primitive)
    {
        try
        {
            return TStrongType.From(primitive);
        }
        catch (StrongTypeValidationException exception)
        {
            throw new JsonException(exception.Message, exception);
        }
    }
}
