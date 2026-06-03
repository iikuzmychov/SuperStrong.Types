using Microsoft.EntityFrameworkCore;
using Npgsql;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters;

public sealed class MinLengthValidatorAdapterNpgsqlTests(ITestOutputHelper testOutputHelper)
    : NpgsqlValidationAdapterTest<MinLengthValidatorAdapterNpgsqlTests.TestDbContext>(testOutputHelper)
{
    [StrongType<string>]
    public sealed partial class TagLabel : IHasStrongTypeDefinition<string>
    {
        public static StrongTypeDefinition<string> Definition => StrongType.Define<string>().HasMinLength(3);
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

    protected override void ConfigureOptions(StrongTypeOptionsBuilder options)
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

    // todo: delete manual implementation once source generators is implemented
    public sealed partial class TagLabel : IStrongType<TagLabel, string>
    {
        private readonly string _value;

        public static StrongTypeLayout<string> Layout => StrongType.Layout<string>();

        private TagLabel(string value)
        {
            _value = value;
        }

        public static TagLabel Create(string value)
        {
            StrongType.EnsureValid(value, Definition);

            return new TagLabel(value);
        }

        public string AsPrimitive() => _value;

        public override int GetHashCode() => _value.GetHashCode();

        public override bool Equals(object? obj) => obj is TagLabel other && _value == other._value;
    }
}
