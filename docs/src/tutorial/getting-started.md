# Getting started

::: warning
SuperStrong.Types is still in **alpha**.
Some features may be incomplete or significantly changed before a stable release.
:::

## Requirements

This library targets **.NET 10**, so your project should target .NET 10 or higher
to be compatible with SuperStrong.Types.

## Installation

First, install the package from [NuGet](https://www.nuget.org/packages/SuperStrong.Types):

::: code-group

```sh-vue [.NET CLI]
dotnet add package SuperStrong.Types --version {{ $frontmatter.version }}
```

```xml-vue [PackageReference]
<PackageReference Include="SuperStrong.Types" Version="{{ $frontmatter.version }}" />
```

:::

This package includes the runtime types, a source generator, analyzers, and code fixes.

## Creating a strong type

To create a strong type you need to mark a partial class with `[StrongType<...>]`:

```csharp
using SuperStrong.Types;

[StrongType<int>]
public sealed partial class OrderId;
```

As a result, the source generator will emit the following code (simplified):

```csharp
[DebuggerDisplay("{_value}")]
[TypeConverter(StrongTypeConverter<OrderId, int>)]
[JsonConverter(JsonStrongTypeConverter<OrderId, int>)]
partial class OrderId :
    IStrongType<OrderId, int>,
    IEquatable<OrderId>, IEqualityOperators<OrderId, OrderId, bool>,
    IComparable<OrderId>, IComparisonOperators<OrderId, OrderId, bool>,
    IParsable<OrderId>, ISpanParsable<OrderId>, IUtf8SpanParsable<OrderId>,
    IFormattable, ISpanFormattable, IUtf8SpanFormattable,
    IConvertible
{
    private readonly int _value;

    private OrderId(int value) => _value = value;

    public static OrderId From(int value) => new(value);

    public int AsPrimitive() => _value;

    public override string ToString() => _value.ToString();

    public sealed override bool Equals(object obj) =>
        obj is OrderId other && Equals(other);

    public bool Equals(OrderId other) => _value.Equals(other._value);

    // ==, !=, <, <=, >, >=
    
    // other interfaces-related members
}
```

[comment]: <> (You can use any type as an underlying primitive. [Learn how] this will affect the generated code.)

## Instantiation

You can create an instance of a strong type from its primitive by using `From(...)`:

```csharp
var orderId = OrderId.From(1);
```

## Parsing

You can also use `Parse(...)` and `TryParse(...)` for parsing a strong type from a string:

```csharp
var orderId = OrderId.Parse("1");
```

```csharp
if (!OrderId.TryParse("invalid input", out var orderId))
{
    // handle failure
}
```

## Casting to a primitive

Sometimes you need to convert a strong type to its underlying primitive. You can achieve this by using `AsPrimitive()`:

```csharp
int raw = orderId.AsPrimitive();
```
