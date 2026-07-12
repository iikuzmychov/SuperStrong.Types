using HotChocolate.Execution;
using HotChocolate.Types;
using HotChocolate.Utilities;
using Microsoft.Extensions.DependencyInjection;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.HotChocolate.Tests.Execution;

public abstract class RoundTripTests<TStrongType, TPrimitive, TSamples>(StrongTypeGraphQLRepresentation representation)
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TSamples : TheoryData<TPrimitive>, new()
{
    public sealed class Query
    {
        public TStrongType Echo(TStrongType input)
        {
            return input;
        }
    }

    public static TheoryData<TPrimitive> PrimitiveSamples { get; } = new TSamples();

    [Theory]
    [MemberData(nameof(PrimitiveSamples))]
    public async Task A_strong_type_value_round_trips_through_an_echo_query_unchanged(TPrimitive primitive)
    {
        var executor = await GraphQLTest.GetExecutorAsync<Query>(representation);
        var scalar = (ScalarType)executor.Schema.QueryType.Fields["echo"].Type.NamedType();
        var converter = executor.Schema.Services.GetRequiredService<ITypeConverter>();

        object runtimeValue = TStrongType.From(primitive);

        if (!scalar.RuntimeType.IsInstanceOfType(runtimeValue))
        {
            runtimeValue = converter.Convert(typeof(object), scalar.RuntimeType, runtimeValue)!;
        }

        var literal = scalar.ValueToLiteral(runtimeValue);

        var result = await executor.ExecuteAsync($"{{ echo(input: {literal}) }}", TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        var value = GraphQLTest.GetData(response, "echo");
        var coerced = scalar.CoerceInputValue(value.Clone(), executor.Schema);

        var roundTripped = coerced switch
        {
            TStrongType strong => strong.AsPrimitive(),
            TPrimitive primitiveValue => primitiveValue,
            _ => converter.Convert<object, TPrimitive>(coerced),
        };

        Assert.Equal(primitive, roundTripped);
    }
}

// char, sbyte, ushort, uint and ulong are omitted: HotChocolate has no default scalar for those CLR types.

public sealed class BoolScalarRoundTripTests()
    : RoundTripTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class ByteScalarRoundTripTests()
    : RoundTripTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class ShortScalarRoundTripTests()
    : RoundTripTests<StrongShort, short, StrongShort.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class IntScalarRoundTripTests()
    : RoundTripTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class LongScalarRoundTripTests()
    : RoundTripTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class FloatScalarRoundTripTests()
    : RoundTripTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class DoubleScalarRoundTripTests()
    : RoundTripTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class DecimalScalarRoundTripTests()
    : RoundTripTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class StringScalarRoundTripTests()
    : RoundTripTests<StrongString, string, StrongString.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class GuidScalarRoundTripTests()
    : RoundTripTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class DateTimeScalarRoundTripTests()
    : RoundTripTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class DateTimeOffsetScalarRoundTripTests()
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class DateOnlyScalarRoundTripTests()
    : RoundTripTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class TimeOnlyScalarRoundTripTests()
    : RoundTripTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class TimeSpanScalarRoundTripTests()
    : RoundTripTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Scalar);

public sealed class BoolPrimitiveRoundTripTests()
    : RoundTripTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class BytePrimitiveRoundTripTests()
    : RoundTripTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class ShortPrimitiveRoundTripTests()
    : RoundTripTests<StrongShort, short, StrongShort.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class IntPrimitiveRoundTripTests()
    : RoundTripTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class LongPrimitiveRoundTripTests()
    : RoundTripTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class FloatPrimitiveRoundTripTests()
    : RoundTripTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class DoublePrimitiveRoundTripTests()
    : RoundTripTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class DecimalPrimitiveRoundTripTests()
    : RoundTripTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class StringPrimitiveRoundTripTests()
    : RoundTripTests<StrongString, string, StrongString.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class GuidPrimitiveRoundTripTests()
    : RoundTripTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class DateTimePrimitiveRoundTripTests()
    : RoundTripTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class DateTimeOffsetPrimitiveRoundTripTests()
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class DateOnlyPrimitiveRoundTripTests()
    : RoundTripTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class TimeOnlyPrimitiveRoundTripTests()
    : RoundTripTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);

public sealed class TimeSpanPrimitiveRoundTripTests()
    : RoundTripTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.Primitive);
