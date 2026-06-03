using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.MinLengthValidatorAdapterTests;

public sealed class MinLengthValidatorAdapterCustomConversionNpgsqlTests(ITestOutputHelper testOutputHelper)
    : NpgsqlValidationAdapterTest<MinLengthValidatorAdapterCustomConversionNpgsqlTests.TestDbContext>(testOutputHelper)
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
            modelBuilder
                .Entity<Tag>()
                .Property(tag => tag.Label)
                .HasConversion(
                    label => label.AsPrimitive().ToLowerInvariant(),
                    stored => TagLabel.Create(stored));
        }
    }

    protected override void ConfigureOptions(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new MinLengthValidatorAdapter());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task No_check_constraint_is_emitted_when_property_has_user_supplied_value_converter()
    {
        await using var connection = Context.Database.GetDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = """
            select count(*)
            from pg_constraint constraint_
            join pg_class table_ on constraint_.conrelid = table_.oid
            where table_.relname = 'Tag' and constraint_.contype = 'c'
            """;

        var count = (long)(await command.ExecuteScalarAsync(TestContext.Current.CancellationToken))!;

        Assert.Equal(0, count);
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
