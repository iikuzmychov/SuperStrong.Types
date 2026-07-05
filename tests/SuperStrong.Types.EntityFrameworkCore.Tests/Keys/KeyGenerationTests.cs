using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.Tests;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Keys;

public abstract class KeyGenerationTests<TStrongType, TPrimitive>(DatabaseFixture database)
    : RelationalTest<KeyGenerationTests<TStrongType, TPrimitive>.Context>(database)
    where TStrongType : class, IStrongType<TStrongType, TPrimitive>
    where TPrimitive : notnull
{
    public sealed class Ticket
    {
        public TStrongType Id { get; set; } = null!;
        public string Subject { get; set; } = "";
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Ticket>()
                .Property(ticket => ticket.Id)
                .ValueGeneratedOnAdd();
        }
    }

    [Fact]
    public async Task A_generated_strong_type_key_is_assigned_on_insert()
    {
        await using var context = CreateDbContext();

        var first = new Ticket { Subject = "a" };
        var second = new Ticket { Subject = "b" };

        context.Tickets.AddRange(first, second);

        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        Assert.NotNull(first.Id);
        Assert.NotNull(second.Id);
        Assert.NotEqual(first.Id, second.Id);
    }
}

public sealed class IntKeyGenerationNpgsqlTests(NpgsqlDatabaseFixture database)
    : KeyGenerationTests<StrongInt, int>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class IntKeyGenerationSqlServerTests(SqlServerDatabaseFixture database)
    : KeyGenerationTests<StrongInt, int>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class LongKeyGenerationNpgsqlTests(NpgsqlDatabaseFixture database)
    : KeyGenerationTests<StrongLong, long>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class LongKeyGenerationSqlServerTests(SqlServerDatabaseFixture database)
    : KeyGenerationTests<StrongLong, long>(database), IClassFixture<SqlServerDatabaseFixture>;

public sealed class GuidKeyGenerationNpgsqlTests(NpgsqlDatabaseFixture database)
    : KeyGenerationTests<StrongGuid, Guid>(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class GuidKeyGenerationSqlServerTests(SqlServerDatabaseFixture database)
    : KeyGenerationTests<StrongGuid, Guid>(database), IClassFixture<SqlServerDatabaseFixture>;
