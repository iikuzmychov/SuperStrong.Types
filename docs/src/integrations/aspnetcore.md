# ASP.NET Core

SuperStrong.Types works with [ASP.NET Core](https://learn.microsoft.com/aspnet/core/) out of the box, in both minimal APIs and MVC controllers. No package or setup is required.

## Route and query parameters

You can use strong types as route and query parameters directly. Minimal APIs bind them through the generated `TryParse(...)`, controllers through the generated `TypeConverter`:

::: code-group

```csharp [Minimal API]
app.MapGet("/orders/{id}", (OrderId id) =>
{
    // todo: get order
});
```

```csharp [Controller]
[HttpGet("orders/{id}")]
public Order GetOrder(OrderId id)
{
    // todo: get order
}
```

:::

Query parameters bind the same way.

## Request body

JSON request bodies are handled by the [System.Text.Json integration](./system-text-json.md), so strong types can appear anywhere in a request DTO:

```csharp
public sealed record CreateOrderRequest(UserId CustomerId, Username CreatedBy);
```

::: code-group

```csharp [Minimal API]
app.MapPost("/orders", (CreateOrderRequest request) =>
{
    // todo: create order
});
```

```csharp [Controller]
[HttpPost("orders")]
public Order CreateOrder([FromBody] CreateOrderRequest request)
{
    // todo: create order
}
```

:::

Responses work in reverse: strong types are serialized as their underlying primitives.

## Invalid values

A value that fails parsing or [validation](../tutorial/validation.md) never reaches your handler. ASP.NET Core responds with `400 Bad Request` instead, no matter where the value comes from: a route, a query string, or a request body.

## OpenAPI

To map strong types correctly in a generated OpenAPI document, use the [OpenAPI integration](./openapi.md).
