using SuperStrong.Types;
using SuperStrong.Types.EntityFrameworkCore.Tests.ExampleTypes;
using System.Diagnostics.CodeAnalysis;

namespace SuperStrong.Tests.ExampleTypes;

[StrongType<int, StrongTypeIdTemplate>]
public sealed partial class UserId;

// should auto generate the next code:
partial class UserId :
    IStrongType<UserId, int>,
    IParsable<UserId>,
    ISpanParsable<UserId>,
    IUtf8SpanParsable<UserId>,
    IFormattable,
    ISpanFormattable,
    IUtf8SpanFormattable,
    IEquatable<UserId>,
    IComparable<UserId>
{
    private readonly int _value;

    public static StrongTypeDefinition<int> Definition => StrongTypeIdTemplate.Definition;
    public static StrongTypeLayout<int> Layout => StrongTypeIdTemplate.Layout;

    private UserId(int value)
    {
        _value = value;
    }

    public static UserId Create(int value)
    {
        StrongType.EnsureValid(value, Definition);

        return new UserId(value);
    }

    public static UserId Parse(string s, IFormatProvider? provider)
    {
        var value = int.Parse(s, provider);

        StrongType.EnsureValid(value, Definition);

        return new UserId(value);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out UserId result)
    {
        if (int.TryParse(s, provider, out var value) && StrongType.IsValid(value, Definition))
        {
            result = new UserId(value);
            return true;
        }

        result = default;
        return false;
    }

    public static UserId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        var value = int.Parse(s, provider);

        StrongType.EnsureValid(value, Definition);

        return new UserId(value);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out UserId result)
    {
        if (int.TryParse(s, provider, out var value) && StrongType.IsValid(value, Definition))
        {
            result = new UserId(value);
            return true;
        }

        result = default;
        return false;
    }

    public static UserId Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        var value = int.Parse(utf8Text, provider);

        StrongType.EnsureValid(value, Definition);

        return new UserId(value);
    }

    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out UserId result)
    {
        if (int.TryParse(utf8Text, provider, out var value) && StrongType.IsValid(value, Definition))
        {
            result = new UserId(value);
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
        if (obj is UserId other)
        {
            return Equals(other);
        }

        return false;
    }

    public bool Equals(UserId? other)
    {
        return
            other is not null &&
            _value.Equals(other._value);
    }

    public int CompareTo(UserId? other)
    {
        if (other is null)
        {
            // non-null values are greater than null
            return 1;
        }

        return _value.CompareTo(other._value);
    }

    public static bool operator ==(UserId? left, UserId? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(UserId? left, UserId? right)
    {
        return !(left == right);
    }
}
