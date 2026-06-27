using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Keys;

public abstract partial class StrongTypeKeyTests(DatabaseHarness database)
    : RelationalTest<StrongTypeKeyTests.Context>(database)
{
    [StrongType<int>] public sealed partial class TicketId;

    public sealed class Ticket
    {
        public TicketId Id { get; set; } = null!;
        public string Subject { get; set; } = "";
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Ticket>().Property(ticket => ticket.Id).ValueGeneratedOnAdd();
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task A_database_generated_strong_type_key_is_assigned_on_insert()
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

    [Fact]
    public async Task An_entity_can_be_found_by_its_strong_type_key()
    {
        TicketId id;

        await using (var context = CreateDbContext())
        {
            var ticket = new Ticket { Subject = "a" };
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            id = ticket.Id;
        }

        await using (var context = CreateDbContext())
        {
            var ticket = await context.Tickets.FindAsync([id], TestContext.Current.CancellationToken);

            Assert.NotNull(ticket);
            Assert.Equal("a", ticket.Subject);
        }
    }

    [Fact]
    public async Task An_entity_can_be_removed_by_its_strong_type_key()
    {
        TicketId id;

        await using (var context = CreateDbContext())
        {
            var ticket = new Ticket { Subject = "a" };
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
            id = ticket.Id;
        }

        await using (var context = CreateDbContext())
        {
            var ticket = await context.Tickets.FindAsync([id], TestContext.Current.CancellationToken);
            context.Tickets.Remove(ticket!);
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            Assert.Null(await context.Tickets.FindAsync([id], TestContext.Current.CancellationToken));
        }
    }
}

public sealed class StrongTypeKeyNpgsqlTests(PostgresDatabaseFixture database)
    : StrongTypeKeyTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class StrongTypeKeySqlServerTests(SqlServerDatabaseFixture database)
    : StrongTypeKeyTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
