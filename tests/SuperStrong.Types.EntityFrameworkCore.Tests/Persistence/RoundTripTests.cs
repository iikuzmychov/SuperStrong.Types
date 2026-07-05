using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public abstract class RoundTripTests<TStrongType, TPrimitive, TSamples>(DatabaseFixture database)
    : RelationalTest<RoundTripTests<TStrongType, TPrimitive, TSamples>.Context>(database)
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
    where TSamples : TheoryData<TPrimitive>, new()
{
    public sealed class Entity
    {
        public int Id { get; set; }
        public TStrongType Value { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Entity> Entities => Set<Entity>();
    }

    public static TheoryData<TPrimitive> PrimitiveSamples { get; } = new TSamples();

    [Theory]
    [MemberData(nameof(PrimitiveSamples))]
    public async Task A_strong_type_value_persists_and_reads_back_unchanged(TPrimitive primitive)
    {
        await using (var context = CreateDbContext())
        {
            context.Entities.Add(new() { Value = TStrongType.From(primitive) });

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var entity = await context.Entities.SingleAsync(TestContext.Current.CancellationToken);

            Assert.Equal(primitive, entity.Value.AsPrimitive());
        }
    }
}

public sealed class BoolRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class BoolRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class ByteRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class ByteRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class SByteRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongSByte, sbyte, StrongSByte.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class SByteRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongSByte, sbyte, StrongSByte.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class ShortRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongShort, short, StrongShort.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class ShortRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongShort, short, StrongShort.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class UShortRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongUShort, ushort, StrongUShort.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class UShortRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongUShort, ushort, StrongUShort.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class IntRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class IntRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class UIntRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongUInt, uint, StrongUInt.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class UIntRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongUInt, uint, StrongUInt.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class LongRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class LongRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class ULongRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongULong, ulong, StrongULong.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class ULongRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongULong, ulong, StrongULong.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class FloatRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class FloatRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DoubleRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class DoubleRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DecimalRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class DecimalRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDecimal, decimal, DecimalRoundTripSqlServerTests.Samples>(database), IClassFixture<SqlServerDatabaseFixture>
{
    public sealed class Samples : TheoryData<decimal>
    {
        // SQL Server's default decimal(18,2) — values are kept within range/scale
        public Samples() { Add(-1.5m); Add(0m); Add(12345.67m); Add(-99999999999999.99m); }
    }
}

public sealed class StringRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongString, string, StrongString.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class StringRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongString, string, StrongString.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class CharRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongChar, char, StrongChar.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class CharRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongChar, char, StrongChar.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class GuidRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class GuidRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DateTimeRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongDateTime, DateTime, DateTimeRoundTripNpgsqlTests.Samples>(database), IClassFixture<NpgsqlDatabaseFixture>
{
    public sealed class Samples : TheoryData<DateTime>
    {
        // Npgsql maps DateTime to timestamptz, which requires UTC
        public Samples()
        {
            Add(new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc));
            Add(new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}

public sealed class DateTimeRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DateTimeOffsetRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, DateTimeOffsetRoundTripNpgsqlTests.Samples>(database), IClassFixture<NpgsqlDatabaseFixture>
{
    public sealed class Samples : TheoryData<DateTimeOffset>
    {
        // Npgsql maps DateTimeOffset to timestamptz, which requires offset 0
        public Samples()
        {
            Add(new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.Zero));
            Add(new DateTimeOffset(2026, 6, 27, 0, 0, 0, TimeSpan.Zero));
        }
    }
}

public sealed class DateTimeOffsetRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DateOnlyRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class DateOnlyRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class TimeOnlyRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class TimeOnlyRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class TimeSpanRoundTripNpgsqlTests(NpgsqlDatabaseFixture database)
    : RoundTripTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class TimeSpanRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongTimeSpan, TimeSpan, TimeSpanRoundTripSqlServerTests.Samples>(database), IClassFixture<SqlServerDatabaseFixture>
{
    public sealed class Samples : TheoryData<TimeSpan>
    {
        // SQL Server's time column holds 0..24h, so durations are kept non-negative and below a day
        public Samples() { Add(TimeSpan.Zero); Add(new TimeSpan(1, 2, 3)); Add(new TimeSpan(23, 59, 59)); }
    }
}

