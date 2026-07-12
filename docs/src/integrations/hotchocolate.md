# Hot Chocolate

SuperStrong.Types integrates with [Hot Chocolate](https://chillicream.com/docs/hotchocolate). Strong types are represented in the schema by their underlying primitive scalar or as named custom scalars.

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

To enable the integration, call `AddStrongTypes(...)` on the GraphQL server. The representation is required, so the choice is always explicit:

```csharp
builder.Services
    .AddGraphQL()
    .AddStrongTypes(StrongTypeGraphQLRepresentation.StrongType);
```

Once enabled, every strong type is handled automatically based on its usage, so you don't need to register anything manually.

## Representation

A strong type can be represented in two ways. Pick the one that fits your clients.

`PrimitiveType` replaces the strong type with the scalar of its underlying primitive everywhere it is used (fields, arguments, input object fields, and collection elements):

```graphql
type Query {
  user(id: UUID!): User
}
```

Clients see plain scalars and the strong types stay invisible in the schema. This is the safe choice when the frontend isn't ready for distinct types.

`StrongType` exposes each strong type as a named scalar and references it everywhere the strong type is used:

```graphql
scalar UserId @strongType(primitiveType: "UUID")

type Query {
  user(id: UserId!): User
}
```

Client code generation can turn it into a distinct type, so strong typing carries over to the frontend.

::: tip
In both representations input values go through `From(...)`, so they are always validated.
:::
