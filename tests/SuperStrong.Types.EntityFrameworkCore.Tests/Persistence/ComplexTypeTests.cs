using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Tests.Infrastructure;
using SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Tests.SqlServer;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Persistence;

public abstract partial class ComplexTypeTests(DatabaseHarness database)
    : RelationalTest<ComplexTypeTests.Context>(database)
{
    [StrongType<decimal>] public sealed partial class Amount;
    [StrongType<string>] public sealed partial class Currency;

    public sealed class Money
    {
        public Amount Amount { get; set; } = null!;
        public Currency Currency { get; set; } = null!;
    }

    public sealed class Payment
    {
        public int Id { get; set; }
        public Money Money { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Payment>().ComplexProperty(payment => payment.Money);
    }

    protected override Context CreateDbContext(DbContextOptions<Context> options) => new(options);

    [Fact]
    public async Task Strong_types_inside_a_complex_type_persist_and_read_back()
    {
        await using (var context = CreateDbContext())
        {
            context.Payments.Add(new Payment { Money = new Money { Amount = Amount.From(99.95m), Currency = Currency.From("USD") } });
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var payment = await context.Payments.SingleAsync(TestContext.Current.CancellationToken);
            Assert.Equal(Amount.From(99.95m), payment.Money.Amount);
            Assert.Equal(Currency.From("USD"), payment.Money.Currency);
        }
    }
}

public sealed class ComplexTypeNpgsqlTests(PostgresDatabaseFixture database)
    : ComplexTypeTests(new NpgsqlHarness(database)), IClassFixture<PostgresDatabaseFixture>;

public sealed class ComplexTypeSqlServerTests(SqlServerDatabaseFixture database)
    : ComplexTypeTests(new SqlServerHarness(database)), IClassFixture<SqlServerDatabaseFixture>;
