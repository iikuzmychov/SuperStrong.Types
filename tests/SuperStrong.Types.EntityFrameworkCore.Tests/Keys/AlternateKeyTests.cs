using Microsoft.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Keys;

public abstract partial class AlternateKeyTests(DatabaseFixture database)
    : RelationalTest<AlternateKeyTests.Context>(database)
{
    [StrongType<Guid>] public sealed partial class AccountId;
    [StrongType<string>] public sealed partial class ExternalRef;

    public sealed class Account
    {
        public AccountId Id { get; set; } = null!;
        public ExternalRef ExternalRef { get; set; } = null!;
    }

    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Account> Accounts => Set<Account>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(account =>
            {
                account.HasAlternateKey(entity => entity.ExternalRef);
                account.Property(entity => entity.ExternalRef).HasMaxLength(32);
            });
        }
    }

    [Fact]
    public async Task An_entity_can_be_queried_by_a_strong_type_alternate_key()
    {
        var id = AccountId.From(Guid.Parse("9122e380-5d22-4029-2c33-b393b3b15029"));

        await using (var context = CreateDbContext())
        {
            context.Accounts.Add(new() { Id = id, ExternalRef = ExternalRef.From("a") });

            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using (var context = CreateDbContext())
        {
            var account = await context.Accounts.SingleAsync(
                entity => entity.ExternalRef == ExternalRef.From("a"),
                TestContext.Current.CancellationToken);

            Assert.Equal(id, account.Id);
        }
    }
}

public sealed class AlternateKeyNpgsqlTests(NpgsqlDatabaseFixture database)
    : AlternateKeyTests(database), IClassFixture<NpgsqlDatabaseFixture>;

public sealed class AlternateKeySqlServerTests(SqlServerDatabaseFixture database)
    : AlternateKeyTests(database), IClassFixture<SqlServerDatabaseFixture>;
