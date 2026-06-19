# Hot Chocolate

SuperStrong.Types integrates with [Hot Chocolate](https://chillicream.com/docs/hotchocolate). Strong types are exposed as custom GraphQL scalars.

## Requirements

This integration targets **Hot Chocolate 16**, so make sure your project uses it.

## Installation

First, install the package from [NuGet](https://www.nuget.org/packages/SuperStrong.Types.HotChocolate):

::: code-group

```sh-vue [.NET CLI]
dotnet add package SuperStrong.Types.HotChocolate --version {{ $frontmatter.version }}
```

```xml-vue [PackageReference]
<PackageReference Include="SuperStrong.Types.HotChocolate" Version="{{ $frontmatter.version }}" />
```

:::

## Setup

To enable the integration, call `AddStrongTypes()` on the GraphQL server:

```csharp
builder.Services
    .AddGraphQL()
    .AddStrongTypes();
```

Once enabled, every strong type is automatically exposed as a scalar based on its usage, so you don't need to register anything manually.

The scalar is named after the type, serialized as its underlying primitive, and its input goes through `From(...)`, so it is always validated.
