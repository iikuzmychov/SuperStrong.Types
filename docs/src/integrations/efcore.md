# EF Core

SuperStrong.Types integrates with [Entity Framework Core](https://learn.microsoft.com/ef/core/) to simplify strong type conversion, querying, and more.

## Requirements

This integration targets **EF Core 10**, so make sure your project uses it.

## Installation

First, install the package from [NuGet](https://www.nuget.org/packages/SuperStrong.Types.EntityFrameworkCore):

::: code-group

```sh-vue [.NET CLI]
dotnet add package SuperStrong.Types.EntityFrameworkCore --version {{ $frontmatter.version }}
```

```xml-vue [PackageReference]
<PackageReference Include="SuperStrong.Types.EntityFrameworkCore" Version="{{ $frontmatter.version }}" />
```

:::

## Setup

To enable the integration, call `UseStrongTypes()` after the database provider:

```csharp
optionsBuilder
    .UseSqlServer(connectionString) // use any provider you need
    .UseStrongTypes();
```

Once enabled, every strong type is automatically configured based on its usage, so you don't need to set up value converters manually.

::: tip
When reading from the database, values go through `From(...)`, so they are always validated.
:::

## Querying

Use the `AsPrimitive()` method inside database queries, just like in normal code:

```csharp
var users = await dbContext.Users
    .Where(user => user.Username.AsPrimitive().StartsWith("ihor"))
    .ToListAsync(cancellationToken);
```

Such queries are translated to SQL as if you used the underlying primitives directly.
