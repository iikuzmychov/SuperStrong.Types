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
        
        return TStrongType.From(primitive!);
    }

    public override void Write(Utf8JsonWriter writer, TStrongType value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.AsPrimitive(), options);
    }
}
