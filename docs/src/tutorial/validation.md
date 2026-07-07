# Validation

Most strong types have restrictions that a value must satisfy to be valid.
You can configure such rules with a definition.

## Definition

To configure validation rules, you need to implement the partial `Define()` method on your strong type. You build the definition with `StrongType.Define<...>()` and chain validation rules onto it:

```csharp
using SuperStrong.Types;

[StrongType<int>]
public sealed partial class Age
{
    public static partial StrongTypeDefinition<int> Define() => StrongType
        .Define<int>()
        .IsPositive()
        .HasMaxValue(150);
}
```

If you don't implement `Define()`, the source generator emits an empty definition, so every value [except null](./null-handling.md) will be accepted.

:::: tip
You can also use the **`Quick Actions`** → **`Implement Define()`**.

::: details Screenshot
<ImplementDefineQuickAction />
:::
::::

If you don't want `Define()` to be a part of your type's public API, you can implement it explicitly:

```csharp
using SuperStrong.Types;

[StrongType<int>]
public sealed partial class Age
{
    static StrongTypeDefinition<int> IStrongType<Age, int>.Define() => StrongType
        .Define<int>()
        .IsPositive()
        .HasMaxValue(150);
}
```

:::: tip
You can also use the **`Quick Actions`** → **`Implement Define() explicitly`**.

::: details Screenshot
<ImplementDefineExplicitlyQuickAction />
:::
::::

## Instantiation

Validation runs every time you create an instance. `From(...)` checks the value against the `Definition` and throws if any rule is not satisfied:

```csharp
var age = Age.From(-1); // throws: negative
```

When invalid input is expected, for example when validating user input, use `TryFrom(...)` instead. It does not throw and returns `false` for an invalid value:

```csharp
if (!Age.TryFrom(input, out var age))
{
    // handle invalid input
}
```

## Parsing

Parsing works the same way. Calling `Parse(...)` runs the validation rules too:

```csharp
var age = Age.Parse("200"); // throws: greater than 150
```

`TryParse(...)` does not throw and returns `false` for invalid input:

```csharp
if (!Age.TryParse(input, out var age))
{
    // handle invalid or unparsable input
}
```
