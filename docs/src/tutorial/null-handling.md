# Null handling

When the underlying primitive is a reference type, such as `string`, `null` is never a valid value. How it is treated depends on the method you use to create or parse an instance.

## Instantiation

Passing `null` to `From(...)` throws `ArgumentNullException`:

```csharp
var username = Username.From(null!); // throws ArgumentNullException
```

`TryFrom(...)` will not throw on `null` and instead will return `false`:

```csharp
Username.TryFrom(null!, out var username); // returns false
```

## Parsing

Parsing follows the same rules. `Parse(...)` parses the input into the primitive and then validates it, so a `null` input throws `ArgumentNullException`:

```csharp
var username = Username.Parse(null!); // throws ArgumentNullException
```

`TryParse(...)` will not throw on `null` and instead will return `false`:

```csharp
Username.TryParse(null, out var username); // returns false
```

## Comparison operators

When a strong type is a reference type, the instance itself can be `null`. The ordering operators (`<`, `<=`, `>`, `>=`) require both operands to be non-null and throw `ArgumentNullException` otherwise:

```csharp
Username? name = null;
name < Username.From("bob"); // throws ArgumentNullException
```

The equality operators (`==` and `!=`) treat `null` as a normal value and never throw:

```csharp
Username? name = null;
name == Username.From("bob"); // false
```

## Null propagation

Sometimes you might want to treat `null` as "no value" instead of invalid input and propagate it through. In such a case, you need to use `FromNullable(...)` instead of `From(...)`:

```csharp
Username? username = Username.FromNullable(null); // returns null
```

`TryFromNullable(...)` does the same for the `Try` pattern:

```csharp
Username.TryFromNullable(null, out var username); // returns true, username is null
```
