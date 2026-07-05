using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Primitives;

public abstract partial class CustomColumnTypeTests(DatabaseFixture database)
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

public sealed class CustomColumnTypeNpgsqlTests(NpgsqlDatabaseFixture database)
    : CustomColumnTypeTests(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class CustomColumnTypeSqlServerTests(SqlServerDatabaseFixture database)
    : CustomColumnTypeTests(database), IClassFixture<SqlServerDatabaseFixture>;
