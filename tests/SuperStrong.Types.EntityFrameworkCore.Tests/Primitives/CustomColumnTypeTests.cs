using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Primitives;

public abstract partial class CustomColumnTypeTests(DatabaseHarness database)
    : RelationalTest<CustomColumnTypeTests.Context>(database)
{
    [StrongType<int>] public sealed partial class Counter;

    public sealed class Meter
    {
        public int Id { get; set; }
        public Counter Counter { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Meter> Meters => Set<Meter>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Meter>().Property(meter => meter.Counter).HasColumnType("bigint");
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task A_strong_type_round_trips_through_an_explicit_column_type()
    {
        await using (var context = CreateDbContext())
        {
            context.Meters.Add(new Meter { Counter = Counter.From(123) });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var meter = await context.Meters.SingleAsync(TestContext.Current.CancellationToken);
            Assert.Equal(Counter.From(123), meter.Counter);
        }
    }
}

public sealed class CustomColumnTypeNpgsqlTests(PostgresDatabaseFixture database)
    : CustomColumnTypeTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class CustomColumnTypeSqlServerTests(SqlServerDatabaseFixture database)
    : CustomColumnTypeTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
