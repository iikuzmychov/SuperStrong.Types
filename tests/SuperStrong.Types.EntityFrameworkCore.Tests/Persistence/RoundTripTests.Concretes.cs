using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public sealed class BoolRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class BoolRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongBool, bool, StrongBool.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class ByteRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class ByteRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongByte, byte, StrongByte.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class SByteRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongSByte, sbyte, StrongSByte.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class SByteRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongSByte, sbyte, StrongSByte.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class ShortRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongShort, short, StrongShort.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class ShortRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongShort, short, StrongShort.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class UShortRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongUShort, ushort, StrongUShort.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class UShortRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongUShort, ushort, StrongUShort.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class IntRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class IntRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongInt, int, StrongInt.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class UIntRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongUInt, uint, StrongUInt.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class UIntRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongUInt, uint, StrongUInt.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class LongRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class LongRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongLong, long, StrongLong.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class ULongRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongULong, ulong, StrongULong.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class ULongRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongULong, ulong, StrongULong.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class FloatRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class FloatRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongFloat, float, StrongFloat.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DoubleRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class DoubleRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDouble, double, StrongDouble.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DecimalRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDecimal, decimal, StrongDecimal.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class DecimalRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDecimal, decimal, DecimalRoundTripSqlServerTests.Samples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>
{
    public sealed class Samples : TheoryData<decimal>
    {
        // SQL Server's default decimal(18,2) — values are kept within range/scale.
        public Samples() { Add(-1.5m); Add(0m); Add(12345.67m); Add(-99999999999999.99m); }
    }
}

public sealed class StringRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongString, string, StrongString.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class StringRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongString, string, StrongString.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class CharRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongChar, char, StrongChar.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class CharRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongChar, char, StrongChar.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class GuidRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class GuidRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongGuid, Guid, StrongGuid.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DateTimeRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDateTime, DateTime, DateTimeRoundTripNpgsqlTests.Samples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>
{
    public sealed class Samples : TheoryData<DateTime>
    {
        // Npgsql maps DateTime to timestamptz, which requires UTC.
        public Samples()
        {
            Add(new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc));
            Add(new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}

public sealed class DateTimeRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDateTime, DateTime, StrongDateTime.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DateTimeOffsetRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, DateTimeOffsetRoundTripNpgsqlTests.Samples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>
{
    public sealed class Samples : TheoryData<DateTimeOffset>
    {
        // Npgsql maps DateTimeOffset to timestamptz, which requires offset 0.
        public Samples()
        {
            Add(new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.Zero));
            Add(new DateTimeOffset(2026, 6, 27, 0, 0, 0, TimeSpan.Zero));
        }
    }
}

public sealed class DateTimeOffsetRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, StrongDateTimeOffset.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DateOnlyRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class DateOnlyRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDateOnly, DateOnly, StrongDateOnly.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class TimeOnlyRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class TimeOnlyRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongTimeOnly, TimeOnly, StrongTimeOnly.ValidPrimitiveSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class TimeSpanRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongTimeSpan, TimeSpan, StrongTimeSpan.ValidPrimitiveSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class TimeSpanRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongTimeSpan, TimeSpan, TimeSpanRoundTripSqlServerTests.Samples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>
{
    public sealed class Samples : TheoryData<TimeSpan>
    {
        // SQL Server's time column holds 0..24h, so durations are kept non-negative and below a day
        public Samples() { Add(TimeSpan.Zero); Add(new TimeSpan(1, 2, 3)); Add(new TimeSpan(23, 59, 59)); }
    }
}

