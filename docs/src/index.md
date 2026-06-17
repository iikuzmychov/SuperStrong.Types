# SuperStrong.Types

Strong types for .NET — define once, use everywhere!

## What does this library do?

You write this:

```csharp
[StrongType<Guid>]
public sealed partial class UserId;
```

You get auto-generated:

- wrapping: `_value`, `From(...)`, `TryFrom(...)`, `AsPrimitive()`, `IStrongType<,>`
- equality: `==`, `!=`, `Equals(...)`, `GetHashCode()`, `IEquatable<>`, `IEqualityOperators<,,>`
- comparison: `<`, `<=`, `>`, `>=`, `IComparable<>`, `IComparisonOperators<,,>`
- formatting: `ToString(...)`, `IFormattable`, `ISpanFormattable`, `IUtf8SpanFormattable`
- parsing: `IParsable<>`, `ISpanParsable<>`, `IUtf8SpanParsable<>`
- conversion: `IConvertible`, `TypeConverter`, `JsonConverter` 

You can easily integrate it with:

- ASP.NET Core
- EF Core
- HotChocolate
- OpenAPI
- Dapper
- MessagePack

## Why use this library?

Fighting [primitive obsession](./other/primitive-obsession.md) usually requires a lot of boilerplate.

SuperStrong.Types generates that boilerplate for you, so you can focus on modelling your domain instead of writing repetitive code.

## What's next?

- Find out more in [Getting Started](./introduction/getting-started.md)
- Learn about strong type [validation](./introduction/validation.md) and [customization](./introduction/customization.md)
- Take a look at the available [integrations](./integrations/ef-core.md)
- Compare SuperStrong.Types with [alternatives](./alternatives/strongly-typed-id.md)
- Do 10 push-ups, 10 pull-ups, and 10 squats

## Acknowledgements

SuperStrong.Types is inspired by two excellent libraries:

- [StronglyTypedId](https://github.com/andrewlock/StronglyTypedId) by Andrew Lock
- [Vogen](https://github.com/SteveDunn/Vogen) by Steve Dunn

Thanks to both authors for the ideas and groundwork.
