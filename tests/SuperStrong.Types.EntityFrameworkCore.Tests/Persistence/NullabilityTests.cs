using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public abstract class NullabilityTests(DatabaseHarness database)
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

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task A_null_strong_type_property_persists_and_reads_back_as_null()
    {
        await using (var context = CreateDbContext())
        {
            context.Entities.Add(new Entity { Value = null });
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
            context.Entities.Add(new Entity { Value = StrongString.From("value") });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var entity = await context.Entities.SingleAsync(TestContext.Current.CancellationToken);
            Assert.Equal(StrongString.From("value"), entity.Value);
        }
    }
}

public sealed class NullabilityNpgsqlTests(PostgresDatabaseFixture database)
    : NullabilityTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class NullabilitySqlServerTests(SqlServerDatabaseFixture database)
    : NullabilityTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
