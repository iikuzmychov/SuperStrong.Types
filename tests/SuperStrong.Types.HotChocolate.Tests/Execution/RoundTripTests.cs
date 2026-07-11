using HotChocolate.Execution;
using HotChocolate.Types;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.HotChocolate.Tests.Execution;

public abstract class RoundTripTests<TStrongType, TPrimitive, TSamples>
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

    private static readonly Lazy<Task<IRequestExecutor>> _executor = new(GraphQLTest.CreateExecutorAsync<Query>);

    [Theory]
    [MemberData(nameof(PrimitiveSamples))]
    public async Task A_strong_type_value_round_trips_through_an_echo_query_unchanged(TPrimitive primitive)
    {
        var executor = await _executor.Value;
        var scalar = executor.Schema.Types.GetType<ScalarType>(typeof(TStrongType).Name);
        var literal = scalar.ValueToLiteral(TStrongType.From(primitive));

        var result = await executor.ExecuteAsync($"{{ echo(input: {literal}) }}", TestContext.Current.CancellationToken);
        using var response = GraphQLTest.ToJsonDocument(result);

        var value = GraphQLTest.GetData(response, "echo");
        var roundTripped = Assert.IsType<TStrongType>(scalar.CoerceInputValue(value.Clone(), executor.Schema));
        Assert.Equal(primitive, roundTripped.AsPrimitive());
    }
}

// char, sbyte, ushort, uint and ulong are omitted: HotChocolate has no default scalar for those CLR types.

public sealed class BoolRoundTripTests
    : RoundTripTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples>;

public sealed class ByteRoundTripTests
    : RoundTripTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples>;

public sealed class ShortRoundTripTests
    : RoundTripTests<StrongShort, short, StrongShort.ValidPrimitiveSamples>;

public sealed class IntRoundTripTests
    : RoundTripTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>;

public sealed class LongRoundTripTests
    : RoundTripTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>;

public sealed class FloatRoundTripTests
    : RoundTripTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples>;

public sealed class DoubleRoundTripTests
    : RoundTripTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples>;

public sealed class DecimalRoundTripTests
    : RoundTripTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples>;

public sealed class StringRoundTripTests
    : RoundTripTests<StrongString, string, StrongString.ValidPrimitiveSamples>;

public sealed class GuidRoundTripTests
    : RoundTripTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>;

public sealed class DateTimeRoundTripTests
    : RoundTripTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples>;

public sealed class DateTimeOffsetRoundTripTests
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples>;

public sealed class DateOnlyRoundTripTests
    : RoundTripTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples>;

public sealed class TimeOnlyRoundTripTests
    : RoundTripTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples>;

public sealed class TimeSpanRoundTripTests
    : RoundTripTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples>;
