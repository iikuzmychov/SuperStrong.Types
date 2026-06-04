using Microsoft.EntityFrameworkCore;
using Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.MinValueValidatorAdapterTests;

public sealed partial class MinValueValidatorAdapterNpgsqlTests(PostgresDatabaseFixture database)
    : NpgsqlValidationAdapterTest<MinValueValidatorAdapterNpgsqlTests.TestDbContext>(database)
{
    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new MinValueValidatorAdapterFactory());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task Age_less_than_min_value_violates_check_constraint()
    {
        var exception = await Assert.ThrowsAsync<PostgresException>(
            () => Context.Database.ExecuteSqlAsync(
                $"""insert into "Person" ("Age") values (-1)""",
                TestContext.Current.CancellationToken));

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);
    }

    [Fact]
    public async Task Age_equal_to_min_value_satisfies_check_constraint()
    {
        var rows = await Context.Database.ExecuteSqlAsync(
            $"""insert into "Person" ("Age") values (0)""",
            TestContext.Current.CancellationToken);

        Assert.Equal(1, rows);
    }

    [Fact]
    public async Task Age_greater_than_min_value_satisfies_check_constraint()
    {
        var rows = await Context.Database.ExecuteSqlAsync(
            $"""insert into "Person" ("Age") values (25)""",
            TestContext.Current.CancellationToken);

        Assert.Equal(1, rows);
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>();
        }
    }

    public sealed class Person
    {
        public required int Id { get; init; }
        public required Age Age { get; init; }
    }

    [StrongType<int>]
    public sealed partial class Age : IHasStrongTypeDefinition<int>, IHasStrongTypeLayout<int>
    {
        public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMinValue(0);
        public static StrongTypeLayout<int> Layout => StrongType.Layout<int>();
    }

}
