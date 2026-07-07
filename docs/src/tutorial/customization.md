# Customization

While most of the generated code can't be modified, you can customize some methods.

## ToString()

Override `ToString()` manually, so the source generator will not emit a default implementation for it:

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
You can also use the **`Quick Actions`** → **`Generate ToString()`**.

::: details Screenshot
<GenerateToStringQuickAction />
:::
::::

## Equality

Implement the partial `Equals(T)` method and override `GetHashCode()` manually, so the source generator will not emit a default implementation for those members:

```csharp
[StrongType<string>]
public sealed partial class Currency
{
    public partial bool Equals(Currency? other) =>
        other is not null &&
        string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() =>
        _value.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
```

```csharp
Currency.From("USD") == Currency.From("usd") // true
```

While you can implement the two methods separately, it's better to implement them both.

:::: tip
You can also use the **`Quick Actions`** → **`Generate Equals(T) and GetHashCode()`**.

::: details Screenshot
<GenerateEqualsAndGetHashCodeQuickAction />
:::
::::
