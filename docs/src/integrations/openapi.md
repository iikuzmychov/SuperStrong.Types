# OpenAPI

SuperStrong.Types integrates with the built-in [OpenAPI support](https://learn.microsoft.com/aspnet/core/fundamentals/openapi/overview) in ASP.NET Core. Strong types are represented in the generated document by their underlying primitive, either inline or as a named component.

## Requirements

This integration targets **ASP.NET Core 10**, so make sure your project uses it.

## Installation

First, install the package from [NuGet](https://www.nuget.org/packages/SuperStrong.Types.AspNetCore.OpenApi):

::: code-group

```sh-vue [.NET CLI]
dotnet add package SuperStrong.Types.AspNetCore.OpenApi --version {{ $frontmatter.version }}
```

```xml-vue [PackageReference]
<PackageReference Include="SuperStrong.Types.AspNetCore.OpenApi" Version="{{ $frontmatter.version }}" />
```

:::

## Setup

To enable the integration, call `AddStrongTypes(...)` when configuring OpenAPI. The representation is required, so the choice is always explicit:

```csharp
builder.Services.AddOpenApi(options =>
    options.AddStrongTypes(StrongTypeOpenApiRepresentation.StrongType));
```

## Representation

A strong type can be represented in two ways. Pick the one that fits your frontend.

`PrimitiveType` replaces the strong type with its underlying primitive everywhere it is used (properties, collection elements, dictionary keys and values, parameters, and any nesting of those):

```json
{ "type": "string", "format": "uuid" }
```

The generated client sees a plain primitive. This is the safe choice when the frontend isn't ready for distinct types.

`StrongType` emits a named schema component and references it everywhere the strong type is used:

```json
{ "$ref": "#/components/schemas/UserId" }
```

Client code generation can turn it into a distinct type, so strong typing carries over to the frontend.
