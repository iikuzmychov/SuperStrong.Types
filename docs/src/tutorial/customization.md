# Customization

While most of the generated code can't be modified, it's possible to customize some methods.

## String representation

You can override `ToString()` manually, so the source generator will not emit a default implementation for it: 

```csharp
[StrongType<string>]
public sealed partial class Password
{
    public override string ToString() => "secret";
}
```

```csharp
var password = Password.From("123456");
Console.WriteLine(password); // prints "secret"
```

:::: tip
You can also use the **`Quick Actions`** → **`Override ToString()`**.

::: details Screenshot
![Override ToString() quick action](/img/code-action-override-tostring.png)
:::
::::

## Equality

You can manually implement `Equals(T)` and override `GetHashCode()`, so the source generator will not emit a default implementation for those members:

```csharp
[StrongType<string>]
public sealed partial class Currency
{
    public bool Equals(Currency? other) =>
        other is not null &&
        string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() =>
        _value.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
```

```csharp
Currency.From("USD") == Currency.From("usd") // true
```

While you can override the two methods separately, it's better to override them both.

:::: tip
You can also use the **`Quick Actions`** → **`Override Equals(T) and GetHashCode()`**.

::: details Screenshot
![Override Equals(T) and GetHashCode() quick action](/img/code-action-override-equals-and-gethashcode.png)
:::
::::