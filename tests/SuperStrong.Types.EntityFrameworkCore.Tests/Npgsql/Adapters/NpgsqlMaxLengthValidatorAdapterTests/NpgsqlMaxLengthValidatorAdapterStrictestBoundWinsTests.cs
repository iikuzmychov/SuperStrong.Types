using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.NpgsqlMaxLengthValidatorAdapterTests;

public sealed partial class NpgsqlMaxLengthValidatorAdapterStrictestBoundWinsTests(PostgresDatabaseFixture database)
    : NpgsqlValidatorAdapterTest<NpgsqlMaxLengthValidatorAdapterStrictestBoundWinsTests.TestDbContext>(database)
{
    [StrongType<string>]
    public sealed partial class UserName : IHasStrongTypeDefinition<string>
    {
        public static StrongTypeDefinition<string> Definition => StrongType.Define<string>().HasMaxLength(20).HasMaxLength(10);
    }

    public sealed class Account
    {
        public required int Id { get; init; }
        public required UserName UserName { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>();
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new NpgsqlMaxLengthValidatorAdapter());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task UserName_column_max_length_matches_strictest_validator_bound()
    {
        var maxLength = await Context.Database
            .SqlQuery<int?>(
                $"""
                select character_maximum_length as "Value"
                from information_schema.columns
                where table_name = 'Account' and column_name = 'UserName'
                """)
            .SingleAsync(TestContext.Current.CancellationToken);

        Assert.Equal(10, maxLength);
    }

}
