using Microsoft.AspNetCore.Builder;
using SuperStrong.Types.Tests;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;

namespace SuperStrong.Types.AspNetCore.Tests.Binding;

public abstract class ParameterBindingTests<TStrongType, TPrimitive, TValidValueSamples, TInvalidValueSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TValidValueSamples : notnull, TheoryData<string, TPrimitive>, new()
    where TInvalidValueSamples : notnull, TheoryData<string>, new()
{
    public static TheoryData<string, TPrimitive> ValidSamples { get; } = new TValidValueSamples();
    public static TheoryData<string> InvalidSamples { get; } = new TInvalidValueSamples();

    private protected abstract Task<TestApplication> StartRouteApplicationAsync();

    private protected abstract Task<TestApplication> StartQueryApplicationAsync();

    [Theory]
    [MemberData(nameof(ValidSamples))]
    public async Task A_route_parameter_binds_a_valid_value(string value, TPrimitive primitive)
    {
        await using var application = await StartRouteApplicationAsync();

        using var response = await application.Client.GetAsync($"/route/{value}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(primitive, await response.Content.ReadFromJsonAsync<TPrimitive>(TestContext.Current.CancellationToken));
    }

    [Theory]
    [MemberData(nameof(InvalidSamples))]
    public async Task A_route_parameter_returns_400_for_an_invalid_value(string value)
    {
        await using var application = await StartRouteApplicationAsync();

        using var response = await application.Client.GetAsync($"/route/{value}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(ValidSamples))]
    public async Task A_query_parameter_binds_a_valid_value(string value, TPrimitive primitive)
    {
        await using var application = await StartQueryApplicationAsync();

        using var response = await application.Client.GetAsync($"/query?value={value}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(primitive, await response.Content.ReadFromJsonAsync<TPrimitive>(TestContext.Current.CancellationToken));
    }

    [Theory]
    [MemberData(nameof(InvalidSamples))]
    public async Task A_query_parameter_returns_400_for_an_invalid_value(string value)
    {
        await using var application = await StartQueryApplicationAsync();

        using var response = await application.Client.GetAsync($"/query?value={value}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

public abstract class MinimalApiParameterBindingTests<TStrongType, TPrimitive, TValidValueSamples, TInvalidValueSamples>
    : ParameterBindingTests<TStrongType, TPrimitive, TValidValueSamples, TInvalidValueSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TValidValueSamples : notnull, TheoryData<string, TPrimitive>, new()
    where TInvalidValueSamples : notnull, TheoryData<string>, new()
{
    private protected override Task<TestApplication> StartRouteApplicationAsync()
    {
        return TestApplication.StartAsync(app =>
        {
            app.MapGet("/route/{value}", static (TStrongType value) => value);
        });
    }

    private protected override Task<TestApplication> StartQueryApplicationAsync()
    {
        return TestApplication.StartAsync(app =>
        {
            app.MapGet("/query", static (TStrongType value) => value);
        });
    }
}

public abstract class ControllerParameterBindingTests<TStrongType, TPrimitive, TValidValueSamples, TInvalidValueSamples>
    : ParameterBindingTests<TStrongType, TPrimitive, TValidValueSamples, TInvalidValueSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TValidValueSamples : notnull, TheoryData<string, TPrimitive>, new()
    where TInvalidValueSamples : notnull, TheoryData<string>, new()
{
    private protected override Task<TestApplication> StartRouteApplicationAsync() => TestApplication.StartAsync<RouteController<TStrongType>>();

    private protected override Task<TestApplication> StartQueryApplicationAsync() => TestApplication.StartAsync<QueryController<TStrongType>>();
}

public sealed class BoolParameterBindingTests
    : MinimalApiParameterBindingTests<StrongBool, bool, BoolParameterBindingTests.ValidValueSamples, BoolParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, bool>
    {
        public ValidValueSamples()
        {
            Add("true", true);
            Add("false", false);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
        }
    }
}

public sealed class BoolControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongBool, bool, BoolParameterBindingTests.ValidValueSamples, BoolParameterBindingTests.InvalidValueSamples>;

public sealed class ByteParameterBindingTests
    : MinimalApiParameterBindingTests<StrongByte, byte, ByteParameterBindingTests.ValidValueSamples, ByteParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, byte>
    {
        public ValidValueSamples()
        {
            Add("0", byte.MinValue);
            Add("255", byte.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongByte.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class ByteControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongByte, byte, ByteParameterBindingTests.ValidValueSamples, ByteParameterBindingTests.InvalidValueSamples>;

public sealed class SByteParameterBindingTests
    : MinimalApiParameterBindingTests<StrongSByte, sbyte, SByteParameterBindingTests.ValidValueSamples, SByteParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, sbyte>
    {
        public ValidValueSamples()
        {
            Add("-128", sbyte.MinValue);
            Add("0", 0);
            Add("127", sbyte.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongSByte.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class SByteControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongSByte, sbyte, SByteParameterBindingTests.ValidValueSamples, SByteParameterBindingTests.InvalidValueSamples>;

public sealed class ShortParameterBindingTests
    : MinimalApiParameterBindingTests<StrongShort, short, ShortParameterBindingTests.ValidValueSamples, ShortParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, short>
    {
        public ValidValueSamples()
        {
            Add("-32768", short.MinValue);
            Add("0", 0);
            Add("32767", short.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongShort.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class ShortControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongShort, short, ShortParameterBindingTests.ValidValueSamples, ShortParameterBindingTests.InvalidValueSamples>;

public sealed class UShortParameterBindingTests
    : MinimalApiParameterBindingTests<StrongUShort, ushort, UShortParameterBindingTests.ValidValueSamples, UShortParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, ushort>
    {
        public ValidValueSamples()
        {
            Add("0", ushort.MinValue);
            Add("65535", ushort.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongUShort.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class UShortControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongUShort, ushort, UShortParameterBindingTests.ValidValueSamples, UShortParameterBindingTests.InvalidValueSamples>;

public sealed class IntParameterBindingTests
    : MinimalApiParameterBindingTests<StrongInt, int, IntParameterBindingTests.ValidValueSamples, IntParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, int>
    {
        public ValidValueSamples()
        {
            Add("-2147483648", int.MinValue);
            Add("0", 0);
            Add("2147483647", int.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongInt.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class IntControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongInt, int, IntParameterBindingTests.ValidValueSamples, IntParameterBindingTests.InvalidValueSamples>;

public sealed class UIntParameterBindingTests
    : MinimalApiParameterBindingTests<StrongUInt, uint, UIntParameterBindingTests.ValidValueSamples, UIntParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, uint>
    {
        public ValidValueSamples()
        {
            Add("0", uint.MinValue);
            Add("4294967295", uint.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongUInt.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class UIntControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongUInt, uint, UIntParameterBindingTests.ValidValueSamples, UIntParameterBindingTests.InvalidValueSamples>;

public sealed class LongParameterBindingTests
    : MinimalApiParameterBindingTests<StrongLong, long, LongParameterBindingTests.ValidValueSamples, LongParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, long>
    {
        public ValidValueSamples()
        {
            Add("-9223372036854775808", long.MinValue);
            Add("0", 0);
            Add("9223372036854775807", long.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongLong.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class LongControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongLong, long, LongParameterBindingTests.ValidValueSamples, LongParameterBindingTests.InvalidValueSamples>;

public sealed class ULongParameterBindingTests
    : MinimalApiParameterBindingTests<StrongULong, ulong, ULongParameterBindingTests.ValidValueSamples, ULongParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, ulong>
    {
        public ValidValueSamples()
        {
            Add("0", ulong.MinValue);
            Add("18446744073709551615", ulong.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongULong.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class ULongControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongULong, ulong, ULongParameterBindingTests.ValidValueSamples, ULongParameterBindingTests.InvalidValueSamples>;

public sealed class FloatParameterBindingTests
    : MinimalApiParameterBindingTests<StrongFloat, float, FloatParameterBindingTests.ValidValueSamples, FloatParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, float>
    {
        public ValidValueSamples()
        {
            Add("-1.5", -1.5f);
            Add("0", 0f);
            Add("3.4028235E38", float.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongFloat.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class FloatControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongFloat, float, FloatParameterBindingTests.ValidValueSamples, FloatParameterBindingTests.InvalidValueSamples>;

public sealed class DoubleParameterBindingTests
    : MinimalApiParameterBindingTests<StrongDouble, double, DoubleParameterBindingTests.ValidValueSamples, DoubleParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, double>
    {
        public ValidValueSamples()
        {
            Add("-1.5", -1.5);
            Add("0", 0d);
            Add("1.7976931348623157E308", double.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongDouble.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class DoubleControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongDouble, double, DoubleParameterBindingTests.ValidValueSamples, DoubleParameterBindingTests.InvalidValueSamples>;

public sealed class DecimalParameterBindingTests
    : MinimalApiParameterBindingTests<StrongDecimal, decimal, DecimalParameterBindingTests.ValidValueSamples, DecimalParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, decimal>
    {
        public ValidValueSamples()
        {
            Add("-79228162514264337593543950335", decimal.MinValue);
            Add("0", 0m);
            Add("79228162514264337593543950335", decimal.MaxValue);
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongDecimal.ForbiddenValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}

public sealed class DecimalControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongDecimal, decimal, DecimalParameterBindingTests.ValidValueSamples, DecimalParameterBindingTests.InvalidValueSamples>;

public sealed class StringParameterBindingTests
    : MinimalApiParameterBindingTests<StrongString, string, StringParameterBindingTests.ValidValueSamples, StringParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, string>
    {
        public ValidValueSamples()
        {
            Add("hello", "hello");
            Add("%20", " ");
            Add("caf%C3%A9", "café");
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add(StrongString.ForbiddenValue);
        }
    }
}

public sealed class StringControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongString, string, StringControllerParameterBindingTests.ValidValueSamples, StringParameterBindingTests.InvalidValueSamples>
{
    // MVC's SimpleTypeModelBinder binds whitespace-only route/query values as null before the
    // type converter runs, so unlike the minimal API host there is no " " sample
    public sealed class ValidValueSamples : TheoryData<string, string>
    {
        public ValidValueSamples()
        {
            Add("hello", "hello");
            Add("caf%C3%A9", "café");
        }
    }
}

public sealed class CharParameterBindingTests
    : MinimalApiParameterBindingTests<StrongChar, char, CharParameterBindingTests.ValidValueSamples, CharParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, char>
    {
        public ValidValueSamples()
        {
            Add("a", 'a');
            Add("7", '7');
            Add("%C3%A9", 'é');
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(Uri.EscapeDataString(StrongChar.ForbiddenValue.ToString()));
        }
    }
}

public sealed class CharControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongChar, char, CharParameterBindingTests.ValidValueSamples, CharParameterBindingTests.InvalidValueSamples>;

public sealed class GuidParameterBindingTests
    : MinimalApiParameterBindingTests<StrongGuid, Guid, GuidParameterBindingTests.ValidValueSamples, GuidParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, Guid>
    {
        public ValidValueSamples()
        {
            Add("00000000-0000-0000-0000-000000000000", Guid.Empty);
            Add("12345678-1234-1234-1234-1234567890ab", Guid.Parse("12345678-1234-1234-1234-1234567890ab"));
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongGuid.ForbiddenValue.ToString());
        }
    }
}

public sealed class GuidControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongGuid, Guid, GuidParameterBindingTests.ValidValueSamples, GuidParameterBindingTests.InvalidValueSamples>;

public sealed class DateTimeParameterBindingTests
    : MinimalApiParameterBindingTests<StrongDateTime, DateTime, DateTimeParameterBindingTests.ValidValueSamples, DateTimeParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, DateTime>
    {
        public ValidValueSamples()
        {
            Add("1999-12-31T23%3A59%3A59", new DateTime(1999, 12, 31, 23, 59, 59));
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(Uri.EscapeDataString(StrongDateTime.ForbiddenValue.ToString("s")));
        }
    }
}

public sealed class DateTimeControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongDateTime, DateTime, DateTimeParameterBindingTests.ValidValueSamples, DateTimeParameterBindingTests.InvalidValueSamples>;

public sealed class DateTimeOffsetParameterBindingTests
    : MinimalApiParameterBindingTests<StrongDateTimeOffset, DateTimeOffset, DateTimeOffsetParameterBindingTests.ValidValueSamples, DateTimeOffsetParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, DateTimeOffset>
    {
        public ValidValueSamples()
        {
            Add("1999-12-31T23%3A59%3A59Z", new DateTimeOffset(1999, 12, 31, 23, 59, 59, TimeSpan.Zero));
            Add("1999-12-31T23%3A59%3A59%2B02%3A00", new DateTimeOffset(1999, 12, 31, 23, 59, 59, TimeSpan.FromHours(2)));
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(Uri.EscapeDataString(StrongDateTimeOffset.ForbiddenValue.ToString("O")));
        }
    }
}

public sealed class DateTimeOffsetControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongDateTimeOffset, DateTimeOffset, DateTimeOffsetParameterBindingTests.ValidValueSamples, DateTimeOffsetParameterBindingTests.InvalidValueSamples>;

public sealed class DateOnlyParameterBindingTests
    : MinimalApiParameterBindingTests<StrongDateOnly, DateOnly, DateOnlyParameterBindingTests.ValidValueSamples, DateOnlyParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, DateOnly>
    {
        public ValidValueSamples()
        {
            Add("1999-12-31", new DateOnly(1999, 12, 31));
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(StrongDateOnly.ForbiddenValue.ToString("O"));
        }
    }
}

public sealed class DateOnlyControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongDateOnly, DateOnly, DateOnlyParameterBindingTests.ValidValueSamples, DateOnlyParameterBindingTests.InvalidValueSamples>;

public sealed class TimeOnlyParameterBindingTests
    : MinimalApiParameterBindingTests<StrongTimeOnly, TimeOnly, TimeOnlyParameterBindingTests.ValidValueSamples, TimeOnlyParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, TimeOnly>
    {
        public ValidValueSamples()
        {
            Add("23%3A59%3A59", new TimeOnly(23, 59, 59));
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(Uri.EscapeDataString(StrongTimeOnly.ForbiddenValue.ToString("O")));
        }
    }
}

public sealed class TimeOnlyControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongTimeOnly, TimeOnly, TimeOnlyParameterBindingTests.ValidValueSamples, TimeOnlyParameterBindingTests.InvalidValueSamples>;

public sealed class TimeSpanParameterBindingTests
    : MinimalApiParameterBindingTests<StrongTimeSpan, TimeSpan, TimeSpanParameterBindingTests.ValidValueSamples, TimeSpanParameterBindingTests.InvalidValueSamples>
{
    public sealed class ValidValueSamples : TheoryData<string, TimeSpan>
    {
        public ValidValueSamples()
        {
            Add("1.02%3A03%3A04", new TimeSpan(1, 2, 3, 4));
            Add("-01%3A00%3A00", TimeSpan.FromHours(-1));
        }
    }

    public sealed class InvalidValueSamples : TheoryData<string>
    {
        public InvalidValueSamples()
        {
            Add("invalid");
            Add(Uri.EscapeDataString(StrongTimeSpan.ForbiddenValue.ToString("c")));
        }
    }
}

public sealed class TimeSpanControllerParameterBindingTests
    : ControllerParameterBindingTests<StrongTimeSpan, TimeSpan, TimeSpanParameterBindingTests.ValidValueSamples, TimeSpanParameterBindingTests.InvalidValueSamples>;
