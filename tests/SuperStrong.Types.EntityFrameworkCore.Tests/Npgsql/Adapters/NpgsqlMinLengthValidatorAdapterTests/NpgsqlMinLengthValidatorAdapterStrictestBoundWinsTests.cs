using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.NpgsqlMinLengthValidatorAdapterTests;

public sealed partial class NpgsqlMinLengthValidatorAdapterStrictestBoundWinsTests(PostgresDatabaseFixture database)
    : NpgsqlValidatorAdapterTest<NpgsqlMinLengthValidatorAdapterStrictestBoundWinsTests.TestDbContext>(database)
{
    [StrongType<string>]
    public sealed partial class TagLabel : IHasStrongTypeDefinition<string>
    {
        public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>().HasMinLength(2).HasMinLength(5);
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
        options.AddValidatorAdapter(new EntityFrameworkCore.Npgsql.Adapters.NpgsqlMinLengthValidatorAdapter());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task Label_shorter_than_strictest_min_length_violates_check_constraint()
    {
        var exception = await Assert.ThrowsAsync<PostgresException>(
            () => Context.Database.ExecuteSqlAsync(
                $"""insert into "Tag" ("Label") values ('abcd')""",
                TestContext.Current.CancellationToken));

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);
    }

    [Fact]
    public async Task Label_at_strictest_min_length_satisfies_check_constraint()
    {
        var rows = await Context.Database.ExecuteSqlAsync(
            $"""insert into "Tag" ("Label") values ('abcde')""",
            TestContext.Current.CancellationToken);

        Assert.Equal(1, rows);
    }

}
