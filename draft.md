# SuperStrong

## SuperStrong.Types

- Strong types instead of weak types with ability to "export" validation logic to reuse it

## SuperStrong.Types.EntityFrameworkCore

- MaxLenght and etc automatic configuration for strong types
- Convertors generating for strong types
- Interceptor for .AsPrimitive() handling in queries for strong types

## SuperStrong.Types.HotChocolate

- Automatic GraphQL type generation for strong types
- Declaration of strong types in GraphQL schema with custom scalars and directives for validation (for FE code generation)

## SuperStrong.Types.AspNetCore.OpenApi

- Automatic OpenAPI schema generation for strong types
- Declaration of strong types in OpenAPI schema with custom scalars and validation attributes (for FE code generation)

## SuperStrong.Modality

- Nullable-like types with custom meaning (Optional, Unknownable, etc.)

## SuperStrong.Guards

- Fody.NullGuard analog
- Out of range enum values guard
- Default structs guard