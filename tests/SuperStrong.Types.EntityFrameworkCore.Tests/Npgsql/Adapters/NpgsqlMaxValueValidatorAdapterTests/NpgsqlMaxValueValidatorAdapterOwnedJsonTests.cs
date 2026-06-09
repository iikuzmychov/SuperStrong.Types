using Microsoft.EntityFrameworkCore;
using SuperStrong.Types.EntityFrameworkCore.Npgsql.Adapters;

namespace SuperStrong.Types.EntityFrameworkCore.Tests.Npgsql.Adapters.NpgsqlMaxValueValidatorAdapterTests;

public sealed partial class NpgsqlMaxValueValidatorAdapterOwnedJsonTests(PostgresDatabaseFixture database)
    : NpgsqlValidatorAdapterTest<NpgsqlMaxValueValidatorAdapterOwnedJsonTests.TestDbContext>(database)
{
    [StrongType<int>]
    public sealed partial class Score
    {
        public static StrongTypeDefinition<int> Definition { get; } = StrongType.Define<int>().HasMaxValue(100);
    }

    public sealed class Stats
    {
        public required Score Score { get; init; }
    }

    public sealed class Player
    {
        public required int Id { get; init; }
        public required Stats Stats { get; init; }
    }

    public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .OwnsOne(player => player.Stats, stats => stats.ToJson());
        }
    }

    protected override void ConfigureStrongTypes(StrongTypeOptionsBuilder options)
    {
        options.AddValidatorAdapter(new NpgsqlMaxValueValidatorAdapterFactory());
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
                where table_.relname = 'Player' and constraint_.contype = 'c'
                """)
            .SingleAsync(TestContext.Current.CancellationToken);

        Assert.Equal(0, checkConstraintCount);
    }

}
