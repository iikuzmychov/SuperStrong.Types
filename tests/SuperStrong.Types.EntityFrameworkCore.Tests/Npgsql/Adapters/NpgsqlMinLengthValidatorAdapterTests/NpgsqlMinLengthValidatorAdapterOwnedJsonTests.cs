using Microsoft.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.NpgsqlMinLengthValidatorAdapterTests;

public sealed partial class NpgsqlMinLengthValidatorAdapterOwnedJsonTests(PostgresDatabaseFixture database)
    : NpgsqlValidatorAdapterTest<NpgsqlMinLengthValidatorAdapterOwnedJsonTests.TestDbContext>(database)
{
    [StrongType<string>]
    public sealed partial class TagLabel : IHasStrongTypeDefinition<string>
    {
        public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>().HasMinLength(3);
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
        options.AddValidatorAdapter(new EntityFrameworkCore.Npgsql.Adapters.NpgsqlMinLengthValidatorAdapter());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task No_check_constraint_is_emitted_when_property_lives_in_json_owned_entity()
    {
        var checkConstraintCount = await Context.Database
            .SqlQuery<long>(
                $"""
                select count(*) as "Value"
                from pg_constraint constraint_
                join pg_class table_ on constraint_.conrelid = table_.oid
                where table_.relname = 'Tag' and constraint_.contype = 'c'
                """)
            .SingleAsync(TestContext.Current.CancellationToken);

        Assert.Equal(0, checkConstraintCount);
    }

}
