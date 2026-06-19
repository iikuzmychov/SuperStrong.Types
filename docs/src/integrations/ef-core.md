# EF Core

SuperStrong.Types integrates with [Entity Framework Core](https://learn.microsoft.com/ef/core/) to simplify strong types conversion, querying and more.

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

Once enabled, every strong type is automatically configured based on its usage, so you don't need to set up value converters or anything else.

::: tip
When reading from the database, values go through `From(...)`, so they are always validated.
:::

## Querying

You can use `AsPrimitive()` method inside database queries as well as in the normal code:

```csharp
var users = dbContext.Users
    .Select(user => user.Username.AsPrimitive().StartsWith("ihor"))
    .ToListAsync(cancellationToken);
```

Such queries will be successfully translated to the SQL as if you would use underlying primitives directly.

