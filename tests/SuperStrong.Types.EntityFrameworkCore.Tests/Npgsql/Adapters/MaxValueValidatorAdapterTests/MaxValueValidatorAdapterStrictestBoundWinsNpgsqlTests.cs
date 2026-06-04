using Microsoft.EntityFrameworkCore;
using Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.MaxValueValidatorAdapterTests;

public sealed partial class MaxValueValidatorAdapterStrictestBoundWinsNpgsqlTests(PostgresDatabaseFixture database)
    : NpgsqlValidationAdapterTest<MaxValueValidatorAdapterStrictestBoundWinsNpgsqlTests.TestDbContext>(database)
{
    [StrongType<int>]
    public sealed partial class Score : IHasStrongTypeDefinition<int>
    {
        public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMaxValue(100).HasMaxValue(50);
    }

    public sealed class Player
    {
        public required int Id { get; init; }
        public required Score Score { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>();
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new MaxValueValidatorAdapterFactory());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task Score_above_strictest_max_value_violates_check_constraint()
    {
        var exception = await Assert.ThrowsAsync<PostgresException>(
            () => Context.Database.ExecuteSqlAsync(
                $"""insert into "Player" ("Score") values (51)""",
                TestContext.Current.CancellationToken));

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);
    }

    [Fact]
    public async Task Score_at_strictest_max_value_satisfies_check_constraint()
    {
        var rows = await Context.Database.ExecuteSqlAsync(
            $"""insert into "Player" ("Score") values (50)""",
            TestContext.Current.CancellationToken);

        Assert.Equal(1, rows);
    }

}
