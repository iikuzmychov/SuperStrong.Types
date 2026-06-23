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

Strong types aren't annotated for Newtonsoft.Json automatically, so you should either annotate the strong type manually:

```csharp
[StrongType<int>]
[JsonConverter(typeof(JsonStrongTypeConverter))] // <-- here
public sealed partial class OrderId;
```

or register the converter on your settings:

```csharp
var settings = new JsonSerializerSettings
{
    Converters = { new JsonStrongTypeConverter() },
};
```

::: warning
Don't confuse with the System.Text.Json attribute and converter of the same name.
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
