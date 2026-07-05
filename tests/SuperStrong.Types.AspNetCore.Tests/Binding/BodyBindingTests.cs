using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using SuperStrong.Types.Tests;
using System.Net;
using System.Net.Http.Json;

namespace SuperStrong.Types.AspNetCore.Tests.Binding;

public abstract class BodyBindingTests<TStrongType, TPrimitive, TValidPrimitiveSamples, TInvalidPrimitiveSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TValidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
    where TInvalidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
{
    public static TheoryData<TPrimitive> ValidPrimitiveSamples { get; } = new TValidPrimitiveSamples();
    public static TheoryData<TPrimitive> InvalidPrimitiveSamples { get; } = new TInvalidPrimitiveSamples();

    private protected abstract Task<TestApplication> StartApplicationAsync();

    [Theory]
    [MemberData(nameof(ValidPrimitiveSamples))]
    public async Task A_body_parameter_binds_a_valid_value(TPrimitive primitive)
    {
        await using var application = await StartApplicationAsync();

        using var response = await application.Client.PostAsJsonAsync(
            "/body",
            primitive,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(primitive, await response.Content.ReadFromJsonAsync<TPrimitive>(TestContext.Current.CancellationToken));
    }

    [Theory(SkipTestWithoutData = true)]
    [MemberData(nameof(InvalidPrimitiveSamples))]
    public async Task A_body_parameter_returns_400_for_an_invalid_value(TPrimitive primitive)
    {
        await using var application = await StartApplicationAsync();

        using var response = await application.Client.PostAsJsonAsync(
            "/body",
            primitive,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

public abstract class MinimalApiBodyBindingTests<TStrongType, TPrimitive, TValidPrimitiveSamples, TInvalidPrimitiveSamples>
    : BodyBindingTests<TStrongType, TPrimitive, TValidPrimitiveSamples, TInvalidPrimitiveSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TValidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
    where TInvalidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
{
    private protected override Task<TestApplication> StartApplicationAsync()
    {
        return TestApplication.StartAsync(app =>
        {
            app.MapPost("/body", ([FromBody] TStrongType value) => value);
        });
    }
}

public abstract class ControllerBodyBindingTests<TStrongType, TPrimitive, TValidPrimitiveSamples, TInvalidPrimitiveSamples>
    : BodyBindingTests<TStrongType, TPrimitive, TValidPrimitiveSamples, TInvalidPrimitiveSamples>
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TValidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
    where TInvalidPrimitiveSamples : notnull, TheoryData<TPrimitive>, new()
{
    private protected override Task<TestApplication> StartApplicationAsync() => TestApplication.StartAsync<BodyController<TStrongType>>();
}

public sealed class BoolBodyBindingTests
    : MinimalApiBodyBindingTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples, StrongBool.InvalidPrimitiveSamples>;

public sealed class BoolControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples, StrongBool.InvalidPrimitiveSamples>;

public sealed class ByteBodyBindingTests
    : MinimalApiBodyBindingTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples, StrongByte.InvalidPrimitiveSamples>;

public sealed class ByteControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples, StrongByte.InvalidPrimitiveSamples>;

public sealed class SByteBodyBindingTests
    : MinimalApiBodyBindingTests<StrongSByte, sbyte, StrongSByte.ValidPrimitiveSamples, StrongSByte.InvalidPrimitiveSamples>;

public sealed class SByteControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongSByte, sbyte, StrongSByte.ValidPrimitiveSamples, StrongSByte.InvalidPrimitiveSamples>;

public sealed class ShortBodyBindingTests
    : MinimalApiBodyBindingTests<StrongShort, short, StrongShort.ValidPrimitiveSamples, StrongShort.InvalidPrimitiveSamples>;

public sealed class ShortControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongShort, short, StrongShort.ValidPrimitiveSamples, StrongShort.InvalidPrimitiveSamples>;

public sealed class UShortBodyBindingTests
    : MinimalApiBodyBindingTests<StrongUShort, ushort, StrongUShort.ValidPrimitiveSamples, StrongUShort.InvalidPrimitiveSamples>;

public sealed class UShortControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongUShort, ushort, StrongUShort.ValidPrimitiveSamples, StrongUShort.InvalidPrimitiveSamples>;

public sealed class IntBodyBindingTests
    : MinimalApiBodyBindingTests<StrongInt, int, StrongInt.ValidPrimitiveSamples, StrongInt.InvalidPrimitiveSamples>;

public sealed class IntControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongInt, int, StrongInt.ValidPrimitiveSamples, StrongInt.InvalidPrimitiveSamples>;

public sealed class UIntBodyBindingTests
    : MinimalApiBodyBindingTests<StrongUInt, uint, StrongUInt.ValidPrimitiveSamples, StrongUInt.InvalidPrimitiveSamples>;

public sealed class UIntControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongUInt, uint, StrongUInt.ValidPrimitiveSamples, StrongUInt.InvalidPrimitiveSamples>;

public sealed class LongBodyBindingTests
    : MinimalApiBodyBindingTests<StrongLong, long, StrongLong.ValidPrimitiveSamples, StrongLong.InvalidPrimitiveSamples>;

public sealed class LongControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongLong, long, StrongLong.ValidPrimitiveSamples, StrongLong.InvalidPrimitiveSamples>;

public sealed class ULongBodyBindingTests
    : MinimalApiBodyBindingTests<StrongULong, ulong, StrongULong.ValidPrimitiveSamples, StrongULong.InvalidPrimitiveSamples>;

public sealed class ULongControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongULong, ulong, StrongULong.ValidPrimitiveSamples, StrongULong.InvalidPrimitiveSamples>;

public sealed class FloatBodyBindingTests
    : MinimalApiBodyBindingTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples, StrongFloat.InvalidPrimitiveSamples>;

public sealed class FloatControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples, StrongFloat.InvalidPrimitiveSamples>;

public sealed class DoubleBodyBindingTests
    : MinimalApiBodyBindingTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples, StrongDouble.InvalidPrimitiveSamples>;

public sealed class DoubleControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples, StrongDouble.InvalidPrimitiveSamples>;

public sealed class DecimalBodyBindingTests
    : MinimalApiBodyBindingTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples, StrongDecimal.InvalidPrimitiveSamples>;

public sealed class DecimalControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples, StrongDecimal.InvalidPrimitiveSamples>;

public sealed class StringBodyBindingTests
    : MinimalApiBodyBindingTests<StrongString, string, StrongString.ValidPrimitiveSamples, StrongString.InvalidPrimitiveSamples>;

public sealed class StringControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongString, string, StrongString.ValidPrimitiveSamples, StrongString.InvalidPrimitiveSamples>;

public sealed class CharBodyBindingTests
    : MinimalApiBodyBindingTests<StrongChar, char, StrongChar.ValidPrimitiveSamples, StrongChar.InvalidPrimitiveSamples>;

public sealed class CharControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongChar, char, StrongChar.ValidPrimitiveSamples, StrongChar.InvalidPrimitiveSamples>;

public sealed class GuidBodyBindingTests
    : MinimalApiBodyBindingTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples, StrongGuid.InvalidPrimitiveSamples>;

public sealed class GuidControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples, StrongGuid.InvalidPrimitiveSamples>;

public sealed class DateTimeBodyBindingTests
    : MinimalApiBodyBindingTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples, StrongDateTime.InvalidPrimitiveSamples>;

public sealed class DateTimeControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples, StrongDateTime.InvalidPrimitiveSamples>;

public sealed class DateTimeOffsetBodyBindingTests
    : MinimalApiBodyBindingTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples, StrongDateTimeOffset.InvalidPrimitiveSamples>;

public sealed class DateTimeOffsetControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples, StrongDateTimeOffset.InvalidPrimitiveSamples>;

public sealed class DateOnlyBodyBindingTests
    : MinimalApiBodyBindingTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples, StrongDateOnly.InvalidPrimitiveSamples>;

public sealed class DateOnlyControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples, StrongDateOnly.InvalidPrimitiveSamples>;

public sealed class TimeOnlyBodyBindingTests
    : MinimalApiBodyBindingTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples, StrongTimeOnly.InvalidPrimitiveSamples>;

public sealed class TimeOnlyControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples, StrongTimeOnly.InvalidPrimitiveSamples>;

public sealed class TimeSpanBodyBindingTests
    : MinimalApiBodyBindingTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples, StrongTimeSpan.InvalidPrimitiveSamples>;

public sealed class TimeSpanControllerBodyBindingTests
    : ControllerBodyBindingTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples, StrongTimeSpan.InvalidPrimitiveSamples>;
