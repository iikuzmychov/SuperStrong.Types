# System.Text.Json

SuperStrong.Types supports [System.Text.Json](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/overview) out of the box. No package or setup is required.

## Serialization

The source generator annotates every strong type with a `[JsonConverter]` attribute pointing to the built-in `JsonStrongTypeConverter<TStrongType, TPrimitive>`:

```csharp
[JsonConverter(typeof(JsonStrongTypeConverter<OrderId, int>))] // auto-generated
public sealed partial class OrderId;
```

Serialization writes the underlying primitive:

```csharp
JsonSerializer.Serialize(OrderId.From(1)); // 1
```

Deserialization reads it back into the strong type:

```csharp
JsonSerializer.Deserialize<OrderId>("1"); // OrderId.From(1)
```

::: tip
When reading from JSON, values go through `From(...)`, so they are always validated.
:::
