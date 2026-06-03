using SuperStrong.Types;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SuperStrong.Tests.ExampleTypes;

[StrongType<int>]
public sealed partial class Digit : IHasStrongTypeDefinition<int>, IHasStrongTypeLayout<int>
{
    public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMinValue(0).HasMaxValue(9);
    public static StrongTypeLayout<int> Layout => StrongType.Layout<int>().HasComparisonOperators(true);
}

// should auto generate the next code:
partial class Digit :
    IStrongType<Digit, int>, // implementation reason: required
    IParsable<Digit>, // implementation reason: defaults
    ISpanParsable<Digit>, // implementation reason: defaults
    IUtf8SpanParsable<Digit>, // implementation reason: defaults
    IFormattable, // implementation reason: defaults
    ISpanFormattable, // implementation reason: defaults
    IUtf8SpanFormattable, // implementation reason: defaults
    IEquatable<Digit>, // implementation reason: defaults
    IComparable<Digit>, // implementation reason: defaults
    IComparisonOperators<Digit, Digit, bool>, // implementation reason: explicitly defined
    IEqualityOperators<Digit, Digit, bool> // implementation reason: defaults
{
    private readonly int _value;

    private Digit(int value)
    {
        _value = value;
    }

    public static Digit Create(int value)
    {
        StrongType.EnsureValid(value, Definition);

        return new Digit(value);
    }

    public static Digit Parse(string s, IFormatProvider? provider)
    {
        var value = int.Parse(s, provider);

        StrongType.EnsureValid(value, Definition);

        return new Digit(value);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Digit result)
    {
        if (int.TryParse(s, provider, out var value) && StrongType.IsValid(value, Definition))
        {
            result = new Digit(value);
            return true;
        }

        result = default;
        return false;
    }

    public static Digit Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        var value = int.Parse(s, provider);

        StrongType.EnsureValid(value, Definition);

        return new Digit(value);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Digit result)
    {
        if (int.TryParse(s, provider, out var value) && StrongType.IsValid(value, Definition))
        {
            result = new Digit(value);
            return true;
        }

        result = default;
        return false;
    }

    public static Digit Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        var value = int.Parse(utf8Text, provider);

        StrongType.EnsureValid(value, Definition);

        return new Digit(value);
    }

    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Digit result)
    {
        if (int.TryParse(utf8Text, provider, out var value) && StrongType.IsValid(value, Definition))
        {
            result = new Digit(value);
            return true;
        }

        result = default;
        return false;
    }

    public override string ToString() => _value.ToString();

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value.ToString(format, formatProvider);
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _value.TryFormat(destination, out charsWritten, format, provider);
    }

    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _value.TryFormat(utf8Destination, out bytesWritten, format, provider);
    }

    public int AsPrimitive() => _value;

    public override int GetHashCode() => _value.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is Digit other)
        {
            return Equals(other);
        }

        return false;
    }

    public bool Equals(Digit? other)
    {
        return
            other is not null &&
            _value.Equals(other._value);
    }

    public int CompareTo(Digit? other)
    {
        if (other is null)
        {
            // non-null values are greater than null
            return 1;
        }

        return _value.CompareTo(other._value);
    }

    public static bool operator ==(Digit? left, Digit? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Digit? left, Digit? right)
    {
        return !(left == right);
    }

    public static bool operator <(Digit left, Digit right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left._value < right._value;
    }

    public static bool operator <=(Digit left, Digit right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left._value <= right._value;
    }

    public static bool operator >(Digit left, Digit right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left._value > right._value;
    }

    public static bool operator >=(Digit left, Digit right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left._value >= right._value;
    }
}
