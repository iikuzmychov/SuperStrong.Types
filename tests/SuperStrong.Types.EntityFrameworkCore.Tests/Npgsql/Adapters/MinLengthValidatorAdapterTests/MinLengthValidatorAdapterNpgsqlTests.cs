using Microsoft.EntityFrameworkCore;
using Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.MinLengthValidatorAdapterTests;

public sealed partial class MinLengthValidatorAdapterNpgsqlTests(PostgresDatabaseFixture database)
    : NpgsqlValidationAdapterTest<MinLengthValidatorAdapterNpgsqlTests.TestDbContext>(database)
{
    [StrongType<string>]
    public sealed partial class TagLabel : IHasStrongTypeDefinition<string>, IHasStrongTypeLayout<string>
    {
        public static StrongTypeDefinition<string> Definition => StrongType.Define<string>().HasMinLength(3);
        public static StrongTypeLayout<string> Layout => StrongType.Layout<string>();
    }

    public sealed class Tag
    {
        public int Id { get; private set; }
        public TagLabel Label { get; private set; } = null!;
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>();
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new MinLengthValidatorAdapter());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task Label_shorter_than_min_length_violates_check_constraint()
    {
        var exception = await Assert.ThrowsAsync<PostgresException>(
            () => Context.Database.ExecuteSqlAsync(
                $"""insert into "Tag" ("Label") values ('ab')""",
                TestContext.Current.CancellationToken));

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);
    }

    [Fact]
    public async Task Label_equal_to_min_length_satisfies_check_constraint()
    {
        var rows = await Context.Database.ExecuteSqlAsync(
            $"""insert into "Tag" ("Label") values ('abc')""",
            TestContext.Current.CancellationToken);

        Assert.Equal(1, rows);
    }

    [Fact]
    public async Task Label_longer_than_min_length_satisfies_check_constraint()
    {
        var rows = await Context.Database.ExecuteSqlAsync(
            $"""insert into "Tag" ("Label") values ('abcd')""",
            TestContext.Current.CancellationToken);

        Assert.Equal(1, rows);
    }

}
