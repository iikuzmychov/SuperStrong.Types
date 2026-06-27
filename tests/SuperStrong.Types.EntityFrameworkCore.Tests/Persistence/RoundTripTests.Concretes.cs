using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public sealed class BoolRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongBool, bool, BoolSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class BoolRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongBool, bool, BoolSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class ByteRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongByte, byte, ByteSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class ByteRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongByte, byte, ByteSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class SByteRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongSByte, sbyte, SByteSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class SByteRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongSByte, sbyte, SByteSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class ShortRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongShort, short, ShortSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class ShortRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongShort, short, ShortSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class UShortRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongUShort, ushort, UShortSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class UShortRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongUShort, ushort, UShortSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class IntRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongInt, int, IntSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class IntRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongInt, int, IntSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class UIntRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongUInt, uint, UIntSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class UIntRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongUInt, uint, UIntSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class LongRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongLong, long, LongSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class LongRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongLong, long, LongSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class ULongRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongULong, ulong, ULongSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class ULongRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongULong, ulong, ULongSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class FloatRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongFloat, float, FloatSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class FloatRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongFloat, float, FloatSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DoubleRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDouble, double, DoubleSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class DoubleRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDouble, double, DoubleSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DecimalRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDecimal, decimal, DecimalSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class DecimalRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDecimal, decimal, DecimalSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class StringRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongString, string, StringSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class StringRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongString, string, StringSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class CharRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongChar, char, CharSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class CharRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongChar, char, CharSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class GuidRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongGuid, Guid, GuidSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class GuidRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongGuid, Guid, GuidSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DateTimeRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDateTime, DateTime, DateTimeSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class DateTimeRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDateTime, DateTime, DateTimeSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DateTimeOffsetRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, DateTimeOffsetSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class DateTimeOffsetRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDateTimeOffset, DateTimeOffset, DateTimeOffsetSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class DateOnlyRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongDateOnly, DateOnly, DateOnlySamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class DateOnlyRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongDateOnly, DateOnly, DateOnlySamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class TimeOnlyRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongTimeOnly, TimeOnly, TimeOnlySamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class TimeOnlyRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongTimeOnly, TimeOnly, TimeOnlySamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

public sealed class TimeSpanRoundTripNpgsqlTests(PostgresDatabaseFixture database)
    : RoundTripTests<StrongTimeSpan, TimeSpan, TimeSpanSamples>(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class TimeSpanRoundTripSqlServerTests(SqlServerDatabaseFixture database)
    : RoundTripTests<StrongTimeSpan, TimeSpan, TimeSpanSamples>(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;

