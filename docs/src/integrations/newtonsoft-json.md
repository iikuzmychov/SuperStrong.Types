# Newtonsoft.Json

SuperStrong.Types integrates with [Newtonsoft.Json](https://www.newtonsoft.com/json). Strong types are serialized as their underlying primitives.

## Requirements

This integration targets **Newtonsoft.Json 13**, so make sure your project uses it.

## Installation

First, install the package from [NuGet](https://www.nuget.org/packages/SuperStrong.Types.NewtonsoftJson):

::: code-group

```sh-vue [.NET CLI]
dotnet add package SuperStrong.Types.NewtonsoftJson --version {{ $frontmatter.version }}
```

```xml-vue [PackageReference]
<PackageReference Include="SuperStrong.Types.NewtonsoftJson" Version="{{ $frontmatter.version }}" />
```

:::

## Setup

Strong types aren't annotated for Newtonsoft.Json automatically, so you need to add the converters yourself.

Register the converters on your serializer settings:

```csharp
var settings = new JsonSerializerSettings
{
    Converters =
    {
        new JsonStrongTypeConverter(),
        new JsonStrongTypeDictionaryConverter(),
    },
};
```

Alternatively, use `[JsonConverter(...)]` on a strong type or dictionary member:

```csharp
[StrongType<int>]
[JsonConverter(typeof(JsonStrongTypeConverter))]
public sealed partial class OrderId;
```

```csharp
public sealed class OrderDto
{
    [JsonConverter(typeof(JsonStrongTypeDictionaryConverter))]
    public Dictionary<OrderId, string> Tags { get; set; } = [];
}
```

::: warning
Don't confuse these with the similarly named attribute and converters from `System.Text.Json`.
:::

## Serialization

Serialization writes the underlying primitive:

```csharp
JsonConvert.SerializeObject(OrderId.From(1), settings); // 1
```

Deserialization reads it back into the strong type:

```csharp
JsonConvert.DeserializeObject<OrderId>("1", settings); // OrderId.From(1)
```

::: tip
When reading from JSON, values go through `From(...)`, so they are always validated.
:::
