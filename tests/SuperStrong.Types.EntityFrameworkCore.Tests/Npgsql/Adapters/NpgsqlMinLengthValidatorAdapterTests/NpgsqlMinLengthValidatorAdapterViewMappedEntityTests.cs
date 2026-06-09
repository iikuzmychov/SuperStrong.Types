using Microsoft.EntityFrameworkCore;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.NpgsqlMinLengthValidatorAdapterTests;

public sealed partial class NpgsqlMinLengthValidatorAdapterViewMappedEntityTests(PostgresDatabaseFixture database)
    : NpgsqlValidatorAdapterTest<NpgsqlMinLengthValidatorAdapterViewMappedEntityTests.TestDbContext>(database)
{
    [StrongType<string>]
    public sealed partial class TagLabel
    {
        public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>().HasMinLength(3);
    }

    public sealed class TagView
    {
        public required TagLabel Label { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<TagView>()
                .HasNoKey()
                .ToView("TagView");
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new EntityFrameworkCore.Npgsql.Adapters.NpgsqlMinLengthValidatorAdapter());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task No_check_constraint_is_emitted_when_entity_is_mapped_to_a_view()
    {
        var checkConstraintCount = await Context.Database
            .SqlQuery<long>(
                $"""
                select count(*) as "Value"
                from pg_constraint constraint_
                join pg_class table_ on constraint_.conrelid = table_.oid
                where table_.relname = 'TagView' and constraint_.contype = 'c'
                """)
            .SingleAsync(TestContext.Current.CancellationToken);

        Assert.Equal(0, checkConstraintCount);
    }

}
