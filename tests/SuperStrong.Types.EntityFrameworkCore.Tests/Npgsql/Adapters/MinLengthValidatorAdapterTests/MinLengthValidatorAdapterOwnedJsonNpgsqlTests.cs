using Microsoft.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.MinLengthValidatorAdapterTests;

public sealed partial class MinLengthValidatorAdapterOwnedJsonNpgsqlTests(PostgresDatabaseFixture database)
    : NpgsqlValidationAdapterTest<MinLengthValidatorAdapterOwnedJsonNpgsqlTests.TestDbContext>(database)
{
    [StrongType<string>]
    public sealed partial class TagLabel : IHasStrongTypeDefinition<string>, IHasStrongTypeLayout<string>
    {
        public static StrongTypeDefinition<string> Definition => StrongType.Define<string>().HasMinLength(3);
        public static StrongTypeLayout<string> Layout => StrongType.Layout<string>();
    }

    public sealed class TagMetadata
    {
        public required TagLabel Label { get; init; }
    }

    public sealed class Tag
    {
        public required int Id { get; init; }
        public required TagMetadata Metadata { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Tag>()
                .OwnsOne(tag => tag.Metadata, metadata => metadata.ToJson());
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new EntityFrameworkCore.Npgsql.Adapters.MinLengthValidatorAdapter());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task No_check_constraint_is_emitted_when_property_lives_in_json_owned_entity()
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

}
