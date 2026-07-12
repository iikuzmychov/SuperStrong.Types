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

public sealed class BoolStrongTypeRoundTripTests()
    : RoundTripTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class ByteStrongTypeRoundTripTests()
    : RoundTripTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class ShortStrongTypeRoundTripTests()
    : RoundTripTests<StrongShort, short, StrongShort.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class IntStrongTypeRoundTripTests()
    : RoundTripTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class LongStrongTypeRoundTripTests()
    : RoundTripTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class FloatStrongTypeRoundTripTests()
    : RoundTripTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class DoubleStrongTypeRoundTripTests()
    : RoundTripTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class DecimalStrongTypeRoundTripTests()
    : RoundTripTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class StringStrongTypeRoundTripTests()
    : RoundTripTests<StrongString, string, StrongString.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class GuidStrongTypeRoundTripTests()
    : RoundTripTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class DateTimeStrongTypeRoundTripTests()
    : RoundTripTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class DateTimeOffsetStrongTypeRoundTripTests()
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class DateOnlyStrongTypeRoundTripTests()
    : RoundTripTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class TimeOnlyStrongTypeRoundTripTests()
    : RoundTripTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class TimeSpanStrongTypeRoundTripTests()
    : RoundTripTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.StrongType);

public sealed class BoolPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class BytePrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class ShortPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongShort, short, StrongShort.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class IntPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class LongPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class FloatPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class DoublePrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class DecimalPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class StringPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongString, string, StrongString.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class GuidPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class DateTimePrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class DateTimeOffsetPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class DateOnlyPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class TimeOnlyPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);

public sealed class TimeSpanPrimitiveTypeRoundTripTests()
    : RoundTripTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples>(StrongTypeGraphQLRepresentation.PrimitiveType);
