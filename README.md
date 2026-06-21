<div align="center">

<img src="https://superstrong.dev/img/logo.png" alt="SuperStrong.Types" width="160" />

# SuperStrong.Types

**Strong types for .NET — define once, use everywhere!**

[![NuGet](https://img.shields.io/nuget/v/SuperStrong.Types?style=for-the-badge&logo=nuget&label=NuGet)](https://www.nuget.org/packages/SuperStrong.Types/)
[![Docs](https://img.shields.io/badge/Docs-online-blue?style=for-the-badge)](https://superstrong.dev/)
[![License](https://img.shields.io/github/license/iikuzmychov/SuperStrong.Types?style=for-the-badge&label=License)](https://github.com/iikuzmychov/SuperStrong.Types/blob/master/LICENSE.md)

</div>

> [!WARNING]
> SuperStrong.Types is in beta: the API and features are not in final shape yet.

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

```sh
dotnet add package SuperStrong.Types --preview
```

This library targets **.NET 10**, so your project should target .NET 10 or higher to be compatible with SuperStrong.Types.

## Why use this library?

Fighting [primitive obsession](https://superstrong.dev/preface/primitive-obsession) usually requires a lot of boilerplate.

SuperStrong.Types generates that boilerplate for you, so you can focus on modelling your domain instead of writing repetitive code.

## What's next?

- Find out more in the [tutorial](https://superstrong.dev/tutorial/getting-started)
- Learn about strong type [validation](https://superstrong.dev/tutorial/validation) and [customization](https://superstrong.dev/tutorial/customization)
- Take a look at the available [integrations](https://superstrong.dev/integrations/ef-core)
- Do 10 push-ups, 10 pull-ups, and 10 squats

## Acknowledgements

SuperStrong.Types is inspired by two excellent libraries:

- [StronglyTypedId](https://github.com/andrewlock/StronglyTypedId) by Andrew Lock
- [Vogen](https://github.com/SteveDunn/Vogen) by Steve Dunn

Thanks to both authors for the ideas and groundwork.
