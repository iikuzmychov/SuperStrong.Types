# Primitive obsession

Primitive obsession is the habit of modelling domain concepts with primitive types instead of giving each concept a type of its own: an id is stored as a `Guid`, a quantity as an `int`, and an email as a `string`.

It's convenient, but primitives carry no meaning and enforce no rules, which quietly leads to bugs.

## Different concepts look the same

A `UserId` and an `OrderId` are both `Guid`, so the compiler can't tell them apart:

```csharp
void CancelOrder(Guid orderId, Guid userId);
```

```csharp
// Arguments swapped, compiles fine, breaks at runtime:
CancelOrder(userId, orderId);
```

The same happens with any same-typed parameters:

```csharp
record User(string FirstName, string LastName);
```

```csharp
// First and last name swapped, no error:
var user = new User("Smith", "John");
```

## Validation is duplicated

When an email is just a `string`, every place that accepts one has to re-check it:

```csharp
if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
{
    throw new ArgumentException("Invalid email.");
}
```

That check gets copy-pasted across controllers, services, and constructors, and it's easy to miss one.

## Intent is lost

A signature like `Charge(string, decimal, string)` tells you nothing. Which string is the currency? What does the `decimal` measure? The types don't say.

## Strong types solve it

A strong type wraps a primitive so each concept becomes its own type. The compiler keeps them apart, validation lives in one place, and the signatures document themselves:

```csharp
[StrongType<Guid>]
public sealed partial class OrderId;
```

```csharp
[StrongType<Guid>]
public sealed partial class UserId;
```

```csharp
void CancelOrder(OrderId orderId, UserId userId);
```

```csharp
CancelOrder(userId, orderId); // does not compile
```

That's the problem SuperStrong.Types is built to solve, without the boilerplate that usually comes with it.
