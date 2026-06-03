using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.MinValueValidatorAdapterTests;

public sealed class MinValueValidatorAdapterViewMappedEntityNpgsqlTests(PostgresDatabaseFixture database)
    : NpgsqlValidationAdapterTest<MinValueValidatorAdapterViewMappedEntityNpgsqlTests.TestDbContext>(database)
{
    [StrongType<int>]
    public sealed partial class Age : IHasStrongTypeDefinition<int>
    {
        public static StrongTypeDefinition<int> Definition => StrongType.Define<int>().HasMinValue(0);
    }

    public sealed class PersonView
    {
        public required Age Age { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PersonView>()
                .HasNoKey()
                .ToView("PersonView");
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(typeof(MinValueValidatorAdapterFactory));
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task No_check_constraint_is_emitted_when_entity_is_mapped_to_a_view()
    {
        await using var connection = Context.Database.GetDbConnection();
        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = """
            select count(*)
            from pg_constraint constraint_
            join pg_class table_ on constraint_.conrelid = table_.oid
            where table_.relname = 'PersonView' and constraint_.contype = 'c'
            """;

        var count = (long)(await command.ExecuteScalarAsync(TestContext.Current.CancellationToken))!;

        Assert.Equal(0, count);
    }

    // todo: delete manual implementation once source generators is implemented
    public sealed partial class Age : IStrongType<Age, int>
    {
        private readonly int _value;

        public static StrongTypeLayout<int> Layout => StrongType.Layout<int>();

        private Age(int value)
        {
            _value = value;
        }

        public static Age Create(int value)
        {
            StrongType.EnsureValid(value, Definition);

            return new Age(value);
        }

        public int AsPrimitive() => _value;

        public override int GetHashCode() => _value.GetHashCode();

        public override bool Equals(object? obj) => obj is Age other && _value == other._value;
    }
}
