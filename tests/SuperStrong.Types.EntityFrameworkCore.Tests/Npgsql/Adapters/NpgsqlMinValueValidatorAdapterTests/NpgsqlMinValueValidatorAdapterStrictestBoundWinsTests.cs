using Microsoft.EntityFrameworkCore;
using Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.NpgsqlMinValueValidatorAdapterTests;

public sealed partial class NpgsqlMinValueValidatorAdapterStrictestBoundWinsTests(PostgresDatabaseFixture database)
    : NpgsqlValidatorAdapterTest<NpgsqlMinValueValidatorAdapterStrictestBoundWinsTests.TestDbContext>(database)
{
    [StrongType<int>]
    public sealed partial class Age : IHasStrongTypeDefinition<int>
    {
        public static StrongTypeDefinition<int> Definition { get; } = StrongType.Define<int>().HasMinValue(0).HasMinValue(18);
    }

    public sealed class Person
    {
        public required int Id { get; init; }
        public required Age Age { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>();
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new NpgsqlMinValueValidatorAdapterFactory());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task Age_below_strictest_min_value_violates_check_constraint()
    {
        var exception = await Assert.ThrowsAsync<PostgresException>(
            () => Context.Database.ExecuteSqlAsync(
                $"""insert into "Person" ("Age") values (17)""",
                TestContext.Current.CancellationToken));

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);
    }

    [Fact]
    public async Task Age_at_strictest_min_value_satisfies_check_constraint()
    {
        var rows = await Context.Database.ExecuteSqlAsync(
            $"""insert into "Person" ("Age") values (18)""",
            TestContext.Current.CancellationToken);

        Assert.Equal(1, rows);
    }

}
