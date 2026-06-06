using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.NpgsqlMinLengthValidatorAdapterTests;

public sealed partial class NpgsqlMinLengthValidatorAdapterCustomConversionTests(PostgresDatabaseFixture database)
    : NpgsqlValidatorAdapterTest<NpgsqlMinLengthValidatorAdapterCustomConversionTests.TestDbContext>(database)
{
    [StrongType<string>]
    public sealed partial class TagLabel : IHasStrongTypeDefinition<string>
    {
        public static StrongTypeDefinition<string> Definition { get; } = StrongType.Define<string>().HasMinLength(3);
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

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new NpgsqlMinLengthValidatorAdapter());
    }

    protected override TestDbContext CreateDbContext(DbContextOptions<TestDbContext> options) => new(options);

    [Fact]
    public async Task No_check_constraint_is_emitted_when_property_has_user_supplied_value_converter()
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
