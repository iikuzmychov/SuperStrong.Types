# SuperStrong.Types

Strong types for .NET — define once, use everywhere!

::: warning
SuperStrong.Types is in **alpha**: the API and features are not in final shape yet.
:::

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
- Hot Chocolate
- Newtonsoft.Json

and more coming soon: OpenAPI, Dapper, MessagePack, ...

## How to install?

Install the package from [NuGet](https://www.nuget.org/packages/SuperStrong.Types):

::: code-group

```sh-vue [.NET CLI]
dotnet add package SuperStrong.Types --version {{ $frontmatter.version }}
```

```xml-vue [PackageReference]
<PackageReference Include="SuperStrong.Types" Version="{{ $frontmatter.version }}" />
```

:::


This library targets **.NET 10**, so your project should target .NET 10 or higher to be compatible with SuperStrong.Types.

## Why use this library?

Fighting [primitive obsession](./preface/primitive-obsession.md) usually requires a lot of boilerplate.

SuperStrong.Types generates that boilerplate for you, so you can focus on modelling your domain instead of writing repetitive code.

## What's next?

- Find out more in the [tutorial](./tutorial/getting-started.md)
- Learn about strong type [validation](./tutorial/validation.md) and [customization](./tutorial/customization.md)
- Take a look at the available [integrations](./integrations/ef-core.md)
- Compare SuperStrong.Types with [alternatives](./alternatives/strongly-typed-id.md)
- Do 10 push-ups, 10 pull-ups, and 10 squats

## Acknowledgements

SuperStrong.Types is inspired by two excellent libraries:

- [StronglyTypedId](https://github.com/andrewlock/StronglyTypedId) by Andrew Lock
- [Vogen](https://github.com/SteveDunn/Vogen) by Steve Dunn

Thanks to both authors for the ideas and groundwork.
