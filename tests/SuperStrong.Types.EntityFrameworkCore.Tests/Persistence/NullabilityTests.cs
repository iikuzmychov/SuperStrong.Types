using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public abstract class NullabilityTests(DatabaseFixture database)
    : RelationalTest<NullabilityTests.Context>(database)
{
    public sealed class Entity
    {
        public int Id { get; set; }
        public StrongString? Value { get; set; }
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Entity> Entities => Set<Entity>();
    }

    [Fact]
    public async Task A_null_strong_type_property_persists_and_reads_back_as_null()
    {
        await using (var context = CreateDbContext())
        {
            context.Entities.Add(new() { Value = null });

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var entity = await context.Entities.SingleAsync(TestContext.Current.CancellationToken);

            Assert.Null(entity.Value);
        }
    }

    [Fact]
    public async Task A_non_null_strong_type_property_reads_back_with_its_value()
    {
        await using (var context = CreateDbContext())
        {
            context.Entities.Add(new() { Value = StrongString.From("value") });

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var entity = await context.Entities.SingleAsync(TestContext.Current.CancellationToken);

            Assert.Equal(StrongString.From("value"), entity.Value);
        }
    }
}

public sealed class NullabilityNpgsqlTests(NpgsqlDatabaseFixture database)
    : NullabilityTests(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class NullabilitySqlServerTests(SqlServerDatabaseFixture database)
    : NullabilityTests(database), IClassFixture<SqlServerDatabaseFixture>;
