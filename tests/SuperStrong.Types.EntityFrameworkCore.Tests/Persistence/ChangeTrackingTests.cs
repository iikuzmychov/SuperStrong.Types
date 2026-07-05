using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public abstract partial class ChangeTrackingTests(DatabaseFixture database)
    : RelationalTest<ChangeTrackingTests.Context>(database)
{
    [StrongType<decimal>] public sealed partial class Amount;

    public sealed class Invoice
    {
        public int Id { get; set; }
        public Amount Total { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Invoice> Invoices => Set<Invoice>();
    }

    [Fact]
    public async Task A_modified_strong_type_property_is_detected_and_persisted()
    {
        await using (var context = CreateDbContext())
        {
            context.Invoices.Add(new Invoice { Total = Amount.From(10m) });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var invoice = await context.Invoices.SingleAsync(TestContext.Current.CancellationToken);
            invoice.Total = Amount.From(42.50m);

            Assert.Equal(1, await context.SaveChangesAsync(TestContext.Current.CancellationToken));
        }

        await using (var context = CreateDbContext())
        {
            var invoice = await context.Invoices.SingleAsync(TestContext.Current.CancellationToken);
            Assert.Equal(Amount.From(42.50m), invoice.Total);
        }
    }

    [Fact]
    public async Task An_unchanged_strong_type_property_produces_no_update()
    {
        await using (var context = CreateDbContext())
        {
            context.Invoices.Add(new Invoice { Total = Amount.From(10m) });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var invoice = await context.Invoices.SingleAsync(TestContext.Current.CancellationToken);
            invoice.Total = Amount.From(10m); // same value, new instance

            Assert.Equal(0, await context.SaveChangesAsync(TestContext.Current.CancellationToken));
        }
    }
}

public sealed class ChangeTrackingNpgsqlTests(NpgsqlDatabaseFixture database)
    : ChangeTrackingTests(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class ChangeTrackingSqlServerTests(SqlServerDatabaseFixture database)
    : ChangeTrackingTests(database), IClassFixture<SqlServerDatabaseFixture>;
